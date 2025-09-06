using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1
{
    public class FinancialComparativeResponse
    {
        public int Month { get; set; } = default!;
        public decimal TotalValuePeriod { get; set; }
        public FinancialTypeEnum FinancialType { get; set; }
    }
}
