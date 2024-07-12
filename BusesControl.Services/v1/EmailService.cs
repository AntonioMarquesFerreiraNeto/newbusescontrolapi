using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BusesControl.Services.v1;

public class EmailService(
    INotificationApi _notificationApi
) : IEmailService
{
    private bool SendEmail(SendEmailDTO sendEmail)
    {
        try
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(AppSettingsEmail.UserName, AppSettingsEmail.Name)
            };

            mail.To.Add(sendEmail.Recipient);
            mail.Subject = sendEmail.Subject;
            mail.Body = sendEmail.HtmlTemplate;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            using SmtpClient smtp = new(AppSettingsEmail.Host, AppSettingsEmail.Port);

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(AppSettingsEmail.UserName, AppSettingsEmail.KeyPassword);
            smtp.Send(mail);

            return true;
        }
        catch (Exception)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.Email.Unexpected
            );
            return false;
        }
    }

    private string RenderTemplateCode(string name, string code, string template)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "{{Name}}", name },
            { "{{Code}}", code },
            { "{{YearNow}}", DateTime.UtcNow.Year.ToString() }
        };

        foreach (var placeholder in placeholders)
        {
            var placeholderPattern = Regex.Escape(placeholder.Key);
            template = Regex.Replace(template, placeholderPattern, placeholder.Value);
        }

        return template;
    }

    private string RenderTemplateWelcome(string name, string link, string template)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "{{FirstName}}", name.Split(' ')[0]},
            { "{{Name}}", name },
            { "{{Link}}", link },
            { "{{YearNow}}", DateTime.UtcNow.Year.ToString() }
        };

        foreach (var placeholder in placeholders)
        {
            var placeholderPattern = Regex.Escape(placeholder.Key);
            template = Regex.Replace(template, placeholderPattern, placeholder.Value);
        }

        return template;
    }

    public bool SendEmailStepCode(string email, string name, string code)
    {
        var basePath = AppContext.BaseDirectory;
        var templatePath = Path.Combine(basePath, "..", "..", "..", "..", "BusesControl.Services", "v1", "Templates", "TemplateCode.html");
        var template = File.ReadAllText(templatePath);

        var sendEmail = new SendEmailDTO
        {
            Subject = "Buses Control - Código de redefinição",
            Recipient = email,
            HtmlTemplate = RenderTemplateCode(name, code, template)
        };

        var success = SendEmail(sendEmail);
        if (!success)
        {
            return false;
        }

        return true;
    }

    public bool SendEmailForWelcomeUserRegistration(string email, string name)
    {
        var basePath = AppContext.BaseDirectory;
        var templatePath = Path.Combine(basePath, "..", "..", "..", "..", "BusesControl.Services", "v1", "Templates", "TemplateWelcomeUserRegistration.html");
        var template = File.ReadAllText(templatePath);

        var sendEmail = new SendEmailDTO
        {
            Subject = "Buses Control - Boas vindas",
            Recipient = email,
            HtmlTemplate = RenderTemplateWelcome(name, "https://buscontrol.netlify.app/user-registration", template)
        };

        var success = SendEmail(sendEmail);
        if (!success)
        {
            return false;
        }

        return true;
    }
}