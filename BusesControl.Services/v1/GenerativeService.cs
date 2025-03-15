using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Json;
using System.Text.Json;

namespace BusesControl.Services.v1
{
    public class GenerativeService(
        INotificationContext _notificationContext,
        AppSettings _appSettings,
        ICacheService _cacheService
    ) : IGenerativeService
    {
        public async Task<GenerativePostResponse> Post(GenerativePostRequest request)
        {
            var payload = TemplatePayloadGenerative.GetPayloadSimpleQuestion(request.Content);

            var cacheKey = string.Format("question:{0}", GenerateBase64.Generate(payload));
            var postResponse = await _cacheService.GetAsync<GenerativePostResponse>(cacheKey);
            if (postResponse != null)
            {
                return postResponse;
            }

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = payload } } } },
                generationConfig = new { temperature = 1, topK = 40, topP = 0.95, maxOutputTokens = 8192, responseMimeType = "text/plain" }
            };

            using var httpClient = new HttpClient();
            var httpResponse = await httpClient.PostAsJsonAsync($"{_appSettings.GoogleGemini.BaseUrl}/v1beta/models/gemini-2.0-flash:generateContent?key={_appSettings.GoogleGemini.ApiToken}", requestBody);
            if (!httpResponse.IsSuccessStatusCode)
            {
                _notificationContext.SetNotification(
                    NotificationTitle.InternalError,
                    Message.Generative.Unexpected,
                    StatusCodes.Status500InternalServerError
                );
                return default!;
            }

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var root = JsonDocument.Parse(jsonResponse);
            var contentResponseJson = root.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString()?.Replace("```json", "").Replace("```", "");
            var contentResponse = JsonSerializer.Deserialize<List<string>>(contentResponseJson!);
            if (contentResponse is null)
            {
                _notificationContext.SetNotification(
                    NotificationTitle.InternalError,
                    Message.Generative.Unexpected,
                    StatusCodes.Status500InternalServerError
                );
                return default!;
            }

            postResponse = new GenerativePostResponse
            {
                Content = contentResponse
            };

            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.Redis.Expire));
            await _cacheService.SetAsync(cacheKey, postResponse, cacheOptions);

            return postResponse;
        }
    }
}
