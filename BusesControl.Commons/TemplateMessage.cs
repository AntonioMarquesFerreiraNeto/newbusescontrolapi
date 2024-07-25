namespace BusesControl.Commons;

public static class TemplateMessage
{
    public static string UserRegistrationQueueReview(string employeeName) => $"Informamos que o funcionário {employeeName} passou pelo processo de registro de usuário e está aguardando aprovação.";

    public static string ContractCompleted(string reference) => $"Informamos que o contrato {reference} foi finalizado recentemente.";
    public static string ContractWaitingReview(string reference) => $"Identificamos que o contrato {reference} está pendente de análise.";
    
    public static string InvoicePaymentPix(string reference) => $"Identificamos que o pagamento da fatura {reference} foi realizado via pix.";
    public static string InvoiceOverDue(string reference) => $"Informamos que a fatura {reference} entrou em estado de atraso neste momento";
}
