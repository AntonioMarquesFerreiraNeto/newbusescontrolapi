namespace BusesControl.Entities.Responses.v1
{
    public class FinancialBalanceResponse
    {
        public decimal ExpenseTotal { get; set; }
        public decimal ExpensePercentage { get; set; }
        public decimal RevenueTotal { get; set; }
        public decimal RevenuePercentage { get; set; }
        public decimal Balance { get; set; }
        public decimal BalancePercentage { get; set; }
    }
}
