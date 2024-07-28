﻿using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Entities.Requests;

public class FinancialExpenseCreateRequest
{
    public Guid SettingPanelId { get; set; }
    public Guid SupplierId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    public DateOnly TerminateDate { get; set; }
}
