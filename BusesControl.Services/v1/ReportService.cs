using AutoMapper;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1
{
    public class ReportService(
        IMapper _mapper,
        IFinancialRepository _financialRepository
    ) : IReportService
    {
        public async Task<IEnumerable<FinancialResponse>> GetFinancialsRecents(int quantities)
        {
            var financialsRecords = await _financialRepository.FindRecentsByQuantities(
                quantities: quantities > 20 ? 20 : quantities
            );

            return _mapper.Map<IEnumerable<FinancialResponse>>(financialsRecords);
        }

        public async Task<IEnumerable<FinancialComparativeResponse>> GetYearlyComparativeAsync() 
        {
            return await _financialRepository.GetYearlyComparativeAsync();
        }
    }
}
