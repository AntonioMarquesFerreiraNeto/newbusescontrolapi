﻿using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface IInvoiceBusiness
{
    Task<InvoiceModel> GetForPaymentAsync(Guid id);
    Task<InvoiceModel> GetForPaymentPixAsync(Guid id, string externalId);
}
