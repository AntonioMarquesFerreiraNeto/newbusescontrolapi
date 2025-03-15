using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces
{
    public interface IGenerativeService
    {
        Task<GenerativePostResponse> Post(GenerativePostRequest request);
    }
}
