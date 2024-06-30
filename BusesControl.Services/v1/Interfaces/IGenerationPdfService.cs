using BusesControl.Entities.Models;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface IGenerationPdfService
{
    Task<PdfCoResponse> GeneratePdfFromTemplateAsync(CustomerContractModel customerContract);
    Task<PdfCoResponse> GeneratePdfTerminationFromTemplateAsync(CustomerContractModel customerContract);
}
