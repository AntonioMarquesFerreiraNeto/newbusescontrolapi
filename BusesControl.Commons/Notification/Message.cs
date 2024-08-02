namespace BusesControl.Commons.Notification;

public class Message
{
    public class Address
    {
        public readonly static string NotFound = "Endereço não encontrado!";
        public readonly static string NotFoundOrInvalid = "Cep não informado ou inválido!";
        public readonly static string Invalid = "Cep informado é inválido!";
    }

    public class Commons
    {
        public readonly static string Unexpected = "Desculpe, ocorreu um erro interno no servidor.";
        public readonly static string BalanceAssasFailure = "Desculpe, ocorreu um erro ao consultar o saldo no assas e essa solicitação.";
        public readonly static string NoOperation = "Nenhuma operação foi necessária para a data especificada ou a atual.";
    }

    public class Customer
    {
        public readonly static string Exists = "Cliente já se encontra registrado!";
        public readonly static string NotFound = "Cliente não encontrado!";
        public readonly static string NotActive = "Cliente não está ativo!";
        public readonly static string Unexpected = "Desculpe, ocorreu um erro interno na criação do cliente.";
    }

    public class CustomerContract
    {
        public readonly static string NotFound = "Contrato do cliente não encontrado!";
        public readonly static string NotPdfContract = "Não é possível gerar o pdf do contrato, o mesmo não está aprovado ou aguardando assinaturas.";
        public readonly static string NotPdfTermination = "Não é possível gerar o pdf da rescisão, o contrato não está em andamento";
        public readonly static string PdfUnexpected = "Desculpe, não conseguimos gerar seu pdf!";
    }

    public class Contract
    {
        public readonly static string NotFound = "Contrato não encontrado!";
        public readonly static string NotIsApproved = "Contrato não está aprovado!";
        public readonly static string SuccessfullyApproved = "Contrato aprovado com sucesso.";
        public readonly static string NotIsDenied = "Contrato não está negado!";
        public readonly static string NotIsWaitingReview = "Contrato não está esperando revisão!";
        public readonly static string NotIsWaitingSignature = "Contrato não está aguardando assinatura!";
        public readonly static string TerminationDateNotInFuture = "A data de término não pode ser menor ou igual a data atual.";
        public readonly static string TerminationDateExceedsLimit = "A data de término não deve exceder o limite de anos estabelecido no painel de configurações para contrato a partir da data atual.";
        public readonly static string InvalidEditRequest = "Contrato não pode ser editado. Ele deve estar negado ou aguardando revisão.";
        public readonly static string InvalidRemoveRequest = "Contrato não pode ser removido. Ele já foi aprovado!";
        public readonly static string EmployeeNotDriver = "Funcionário selecionado não é motorista!";
        public readonly static string DuplicateCustomers = "Pelo menos um cliente está duplicado nesta requisição!";
        public readonly static string NotInProgress = "Contrato não está em andamento!";
        public readonly static string SuccessfullyStartContract = "Contrato inicializado com sucesso! Consulte as faturas dos clientes no módulo financeiro.";
    }

    public class ContractDescription
    {
        public readonly static string NotFound = "Descrição de contrato não encontrado!";
        public readonly static string NotUpdate = "Descrição de contrato é usado em pelo menos um contrato aprovado!";
        public readonly static string NotDelete = "Descrição de contrato é usado em pelo menos um contrato!";
    }

    public class ContractDriverReplacement
    {
        public readonly static string NotFound = "Substituição de motorista não encontrada!";
        public readonly static string StartDateLessTerminateDate = "Data de início não pode ser menor que a data de término.";
        public readonly static string StartDateLessContractStartDate = "Data de início não pode ser menor que a data de início do contrato.";
        public readonly static string TerminateDateGreaterContractTerminateDate = "Data de término não pode ser maior que a data de término do contrato.";
        public readonly static string DriverInvalid = "Não é possível vincular esse motorista!";
    }

    public class ContractBusReplacement
    {
        public readonly static string NotFound = "Substituição de ônibus não encontrada!";
        public readonly static string StartDateLessTerminateDate = "Data de início não pode ser menor que a data de término.";
        public readonly static string StartDateLessContractStartDate = "Data de início não pode ser menor que a data de início do contrato.";
        public readonly static string TerminateDateGreaterContractTerminateDate = "Data de término não pode ser maior que a data de término do contrato.";
        public readonly static string BusInvalid = "Não é possível vincular esse ônibus!";
    }

    public class Color
    {
        public readonly static string NotFound = "Cor não encontrada!";
        public readonly static string NotActive = "Cor selecionada não está ativa!";
        public readonly static string Exists = "Cor já se encontra registrada!";
        public readonly static string ExistsInBus = "Cor é usada em pelo menos um ônibus!";
    }

    public class Bus
    {
        public readonly static string NotFound = "Ônibus não encontrado!";
        public readonly static string NotActive = "Ônibus não está ativo!";
        public readonly static string Invalid = "Operação inválida!";
        public readonly static string Exists = "Ônibus já se encontra registrado!";
        public readonly static string NotAvailable = "Ônibus não está disponível para novos contratos";
    }

    public class Employee
    {
        public readonly static string Exists = "Funcionário já se encontra registrado!";
        public readonly static string NotFound = "Funcionário não encontrado!";
        public readonly static string NotActive = "Funcionário não está ativo!";
        public readonly static string NotDriver = "Funcionário não é um motorista!";
        public readonly static string NoChangeNeeded = "A requisição não resultou em nenhuma alteração, pois o tipo do registro já é o mesmo.";
        public readonly static string RoleChangedNoAccess = "Papel do funcionário alterado com sucesso! Devido ao perfil do mesmo, ele não possui acesso ao sistema.";
        public readonly static string RoleChangedRegisterInQueue = "Papel do funcionário alterado com sucesso, mas se deseja que ele tenha acesso ao sistema, registre o mesmo na fila de registro de usuários.";
        public readonly static string RoleAndProfileChanged = "Papel e perfil do funcionário alterado com sucesso! Agora ele terá novos acessos ao sistema.";
    }

    public class Email
    {
        public readonly static string Unexpected = "Desculpe, ocorreu um erro interno e não conseguimos enviar seu e-mail";
    }

    public class SettingPanel
    {
        public readonly static string NotFound = "Painel de configurações não encontrado!";
        public readonly static string Exists = "Já existe um painel de configurações para o parent especificado!";
        public readonly static string NotUpdate = "Existe contratos aprovados que usam esse painel de configurações!";
        public readonly static string NotDestine = "Não é possível realizar essa vinculação de painel de configuração!";
        public readonly static string NotDelete = "Painel de configurações é usado em pelo menos um contrato ou financeiro!";
    }

    public class User
    {
        public readonly static string NotFound = "Usuário não encontrado!";
        public readonly static string CredentialsInvalid = "E-mail ou senha informados podem estar incorretos!";
        public readonly static string Exists = "Usuário já se encontra registrado!";
        public readonly static string InvalidRole = "Permisão selecionada é inválida!";
        public readonly static string Unexpected = "Desculpe, não conseguimos processar a requisição do usuário!";
        public readonly static string SuccessStepOne = "Olá, tudo bem? Enviamos um código de redefinição de senha para seu e-mail.";
        public readonly static string InvalidCurrentPassword = "Senha atual é diferente da senha informada!";
        public readonly static string InvalidNewPassword = "Nova senha não pode ser igual a atual!";
        public readonly static string SuccessChangePassword = "Senha alterada com sucesso!";
    }

    public class UserRegistration
    {
        public readonly static string SuccessCreate = "Solicitação de registro de usuário realizada com sucesso!";
        public readonly static string SuccessDelete = "Solicitação de registro de usuário deletado com sucesso.";
        public readonly static string SuccessApproved = "Usuário aprovado com sucesso! Agora ele poderá conectar ao sistema buses control.";
        public readonly static string SuccessStepCode = "Olá, tudo bem? Enviamos um código de definição de senha para seu e-mail.";
        public readonly static string NotFound = "Solicitação de registro de usuário não encontrada!";
        public readonly static string CodeNotFound = "Código de usuário não encontrada!";
        public readonly static string InvalidStepCode = "Processo inicial do processo de registro de usuários concluído ou em progresso!";
        public readonly static string InvalidStepPassword = "Processo não está na fase de registro de senhas!";
        public readonly static string CodeInvalid = "Ops, código de redefinição expirado, repita os passos novamente.";
        public readonly static string Unexpected = "Desculpe, não conseguimos gerar seu token!";
        public readonly static string Success = "Senha definida com sucesso, aguarde sua aprovação pelo administrador para realizar o login!";
        public readonly static string InvalidDelete = "Solicitação de cancelamento negada por já estar aprovada!";
        public readonly static string RequestDenied = "Solicitação de registro de usuário negada.";
        public readonly static string Exists = "Já existe uma solicitação.";
        public readonly static string InvalidApproved = "Para aprovar o registro do usuário, é necessário que ele tenha finalizado o processo de criação!";
    }

    public class ResetUser 
    {
        public readonly static string UnexpectedToken = "Código de redefinição não encontrado!";
        public readonly static string CodeInvalid = "Ops, código de redefinição expirado, repita os passos novamente.";
        public readonly static string InvalidPassword = "Nova senha e confirmar senha devem ser iguais!";
        public readonly static string Unexpected = "Desculpe, não conseguimos gerar seu token!";
        public readonly static string Success = "Senha redefinida com sucesso!";
    }

    public class Invoice
    {
        public readonly static string NotFound = "Fatura não encontrada!";
        public readonly static string NotPending = "Fatura não está pendente!";
        public readonly static string UnexpectedCreate = "Desculpe, não conseguimos gerar a fatura do cliente!";
        public readonly static string Unexpected = "Desculpe, não conseguimos tratar a fatura do cliente!";
        public readonly static string UnexpectedPay = "Desculpe, não conseguimos realizar seu pagamento!";
        public readonly static string UnexpectedPix = "Desculpe, não conseguimos gerar seu pix!";
        public readonly static string NotPendingOrOverdue = "Fatura não está pendente ou atrasada!";
        public readonly static string SuccessPay = "Pagamento realizado com sucesso!";
        public readonly static string SuccessPix = "Use o QR code para realizar o pagamento de sua fatura.";
        public readonly static string SuccessJustCount = "Pagamento contabilizado com sucesso!";
        public readonly static string NotCreditCard = "Cartão de crédito não informado.";
        public readonly static string FailureAutomatedPay = "Fatura não está confirmada ou recebida no assas.";
        public readonly static string FailureOverDue = "Fatura não pode estar pendente.";
        public readonly static string SettingPanelNotFound = "Painel de configuração não encontrado.";
        public readonly static string JustCountInternalUsersOnly = "Contabilizar pagamento é apenas para usuários internos!";
    }

    public class InvoiceExpense
    {
        public readonly static string NotFound = "Fatura de despensa não encontrada!";
        public readonly static string NotPending = "Fatura de despesa não está pendente!";
        public readonly static string Unexpected = "Desculpe, não conseguimos tratar a fatura de despesa!";
        public readonly static string UnexpectedPay = "Desculpe, não conseguimos realizar seu pagamento!";
        public readonly static string SuccessPayPix = "Pagamento via pix realizado com sucesso.";
        public readonly static string SuccessPayTed = "Pagamento via TED realizado com sucesso.";
        public readonly static string SuccessPayJustCount = "Pagamento contabilizado com sucesso.";
        public readonly static string InsufficientBalance = "O saldo disponível é insuficiente para o pagamento dessa fatura de despesa!";
    }

    public class Webhook
    {
        public readonly static string NotFound = "Webhook não encontrado!";
        public readonly static string RequestRequired = "Corpo da requisição é obrigatório!";
        public readonly static string Unauthorized = "Requisição não autorizada!";
        public readonly static string Unexpected = "Desculpe, houve uma falha na ação ao integrar com o assas!";
        public readonly static string EventNotAccepted = "O evento recebido não corresponde a nenhum evento conhecido para esse método.";
        public readonly static string ExistsByNameOrUrlOrType = "Já existe um webhook com esse nome, url ou tipo!";
        public readonly static string SuccessDelete = "Webhook deletado com sucesso. O sistema não executará mais as ações que dependiam deste webhook até que um novo webhook compatível seja criado.";
    }

    public class Termination 
    {
        public readonly static string NotProcess = "É necessário iniciar o processo de rescisão!";
        public readonly static string NotActive = "Cliente não está mais ativo neste contrato!";
        public readonly static string Success = "Rescisão concluída com sucesso. Esta ação não pode ser revertida e não isenta o pagamento de faturas que estão em atraso.";
    }

    public class Financial
    {
        public readonly static string NotFound = "Financeiro não encontrado!";
        public static readonly string IsInactive = "Não é possível alterar esse financeiro, ele não está ativo!";
        public readonly static string InvalidInactive = "Não é possível inativar esse financeiro!";
        public readonly static string TerminationDateExceedsLimit = "A data de término não deve exceder o limite de seis anos.";
    }

    public class Supplier
    {
        public readonly static string Exists = "Fornecedor já se encontra registrado!";
        public readonly static string NotFound = "Fornecedor não encontrado!";
        public readonly static string NotActive = "Fornecedor não está ativo!";
    }

    public class Notification
    {
        public readonly static string NotFound = "Notificação não encontrada!";
    }

    public class SupportTicket
    {
        public readonly static string NotFound = "Ticket de suporte não encontrada!";
        public readonly static string NotOpenOrInProgress = "Ticket de suporte não está aberto ou em progresso!";
    }

    public class SupportTicketMessage
    {
        public readonly static string NotFound = "Mensagem de ticket de suporte não encontrada!";
    }
}
