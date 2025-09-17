using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.DTOs
{
    public class FinancialResumeDto
    {
        public decimal TotalPrice { get; set; }
        public FinancialTypeEnum Type { get; set; }
    }
}
