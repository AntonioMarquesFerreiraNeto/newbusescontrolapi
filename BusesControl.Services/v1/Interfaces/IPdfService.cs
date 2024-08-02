using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IPdfService
{
    Task<PdfCoResponse> GeneratePdfFromTemplateAsync(CustomerContractModel customerContract);
    Task<PdfCoResponse> GeneratePdfTerminationFromTemplateAsync(CustomerContractModel customerContract);
}
