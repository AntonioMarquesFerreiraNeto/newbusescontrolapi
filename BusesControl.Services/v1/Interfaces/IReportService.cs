using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<FinancialResponse>> GetFinancialsRecents(int quantities);
        Task<IEnumerable<FinancialComparativeResponse>> GetYearlyComparativeAsync();
        Task<FinancialBalanceResponse> GetBalanceAsync();
    }
}
