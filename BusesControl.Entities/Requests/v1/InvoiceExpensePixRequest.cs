namespace BusesControl.Entities.Requests.v1;

public class InvoiceExpensePixRequest
{
    public string BankCode { get; set; } = default!;
    public string AccountName { get; set; } = default!;
    public string OwnerName { get; set; } = default!;
    public DateOnly OwnerBirthDate { get; set; }
    public string CpfCnpj { get; set; } = default!;
    public string Agency { get; set; } = default!;
    public string Account { get; set; } = default!;
    public string AccountDigit { get; set; } = default!;
    public bool CurrentAccount { get; set; }
}
