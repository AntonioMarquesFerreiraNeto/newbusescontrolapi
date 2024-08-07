﻿using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.DTOs;

public class CreateInvoiceDTO
{
    public Guid FinancialId { get; set; }
    public string CustomerExternalId { get; set; } = default!;
    public string Reference { get; set; } = default!;
    public int Index { get; set; }
    public decimal Price { get; set; }
    public DateOnly DueDate { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PaymentTypeEnum PaymentType { get; set; }
    public FinancialTypeEnum FinancialType { get; set; } = default!;
    public bool IsContract { get; set; }

    public void SetTitleAndDescription()
    {
        var suffix = IsContract ? "do contrato" : "da receita";

        Title = PaymentType == PaymentTypeEnum.Single ? "Fatura única" : $"{Index}º fatura";
        Description = $"{Title} referente ao módulo financeiro {suffix} {Reference}.";
    }
}
