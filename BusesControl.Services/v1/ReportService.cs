using AutoMapper;
using BusesControl.Commons;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1
{
    public class ReportService(
        IMapper _mapper,
        ICacheService _cacheService,
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

        public async Task<IEnumerable<FinancialComparativeResponse>> GetYearlyComparativeAsync(int gapMonth) 
        {
            var cacheKey = string.Format(CacheKey.FinancialYearlyComparative, gapMonth);
            var response = await _cacheService.GetAsync<List<FinancialComparativeResponse>>(cacheKey);
            if (response != null)
            {
                return response;
            }

            var baseDate = DateTime.UtcNow.AddMonths(-(gapMonth - 1));
            var startDate = new DateTime(baseDate.Year, baseDate.Month, 1);

            var financialsRecords = await _financialRepository.GetYearlyComparativeAsync(startDate);

            var financialGroup = financialsRecords.GroupBy(x => new
            {
                x.StartDate.Year,
                x.StartDate.Month,
                x.Type,
            }).ToDictionary(
                group => (group.Key.Year, group.Key.Month, group.Key.Type),
                group => group.Sum(financial => financial.TotalPrice)
            );

            response = [];

            for (var index = 1; index <= gapMonth; index++)
            {
                var keyReceitas = (startDate.Year, startDate.Month, FinancialTypeEnum.Revenue);
                var keyDespesas = (startDate.Year, startDate.Month, FinancialTypeEnum.Expense);

                financialGroup.TryGetValue(keyReceitas, out var receitas);
                financialGroup.TryGetValue(keyDespesas, out var despesas);

                response.Add(new FinancialComparativeResponse
                {
                    FinancialType = FinancialTypeEnum.Revenue,
                    Period = string.Format("{0}", startDate.ToString("MM/yyyy")),
                    TotalValuePeriod = receitas
                });

                response.Add(new FinancialComparativeResponse
                {
                    FinancialType = FinancialTypeEnum.Expense,
                    Period = string.Format("{0}", startDate.ToString("MM/yyyy")),
                    TotalValuePeriod = despesas
                });

                startDate = startDate.AddMonths(1);
            }

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

            return response;
        }

        public async Task<FinancialBalanceResponse> GetBalanceAsync()
        {
            var response = await _financialRepository.GetBalanceAsync();

            var total = response.RevenueTotal +  response.ExpenseTotal;

            response.ExpensePercentage = total != 0 ? Math.Round(response.ExpenseTotal / total * 100, 2) : 0;
            response.RevenuePercentage = total != 0 ? Math.Round(response.RevenueTotal / total * 100, 2) : 0;
            response.BalancePercentage = total != 0 ? Math.Round(response.Balance / total * 100, 2) : 0;

            return response;
        }
    }
}
