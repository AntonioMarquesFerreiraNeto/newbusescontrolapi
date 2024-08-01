namespace BusesControl.Entities.DTOs;

public class CreateInvoiceExpenseInDTO
{
    public int Index { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid FinancialId { get; set; }
    public string FinancialReference { get; set; } = default!;
    public decimal Price { get; set; }
    public DateOnly DueDate { get; set; }
    public bool IsSingle { get; set; }

    public void SetTitleAndDescription()
    {
        Title = IsSingle ? "Fatura única referente a despesa" : $"{Index}º fatura referente a despesa";
        Description = $"Fatura da empresa referente ao módulo financeiro de despesa com fornecedores.";
    }
}
