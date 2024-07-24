namespace BusesControl.Commons;

public static class TemplateNotificationMessage
{
    public static string UserRegistrationQueueReview(string employeeName) => $"Informamos que o funcionário {employeeName} passou pelo processo de registro de usuário e está aguardando aprovação.";

    public static string ContractCompleted(string reference) => $"Informamos que o contrato {reference} foi finalizado recentemente.";
    public static string ContractWaitingReview(string reference) => $"Identificamos que o contrato {reference} está pendente de análise.";
    
    public static string InvoicePaymentPix(string reference, string customerName) => $"Identificamos que o pagamento da fatura {reference} do cliente {customerName} foi realizado via pix.";
    public static string InvoiceOverDue(string reference, string customerName) => $"Informamos que a fatura {reference} do cliente {customerName} entrou em estado de atraso neste momento";
}
