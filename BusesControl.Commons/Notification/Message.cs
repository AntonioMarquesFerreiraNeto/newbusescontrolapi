namespace BusesControl.Commons.Notification;

public class Message
{
    public class Address
    {
        public readonly static string NotFoundOrInvalid = "Cep não informado ou inválido!";
        public readonly static string Invalid = "Cep informado é inválido!";
    }

    public class Commons
    {
        public readonly static string Unexpected = "Desculpe, ocorreu um erro interno no servidor.";
    }

    public class Bus
    {
        public readonly static string NotFound = "Ônibus não encontrado!";
        public readonly static string Invalid = "Operação inválida!";
        public readonly static string Exists = "Ônibus já se encontra registrado!";
    }

    public class Employee
    {
        public readonly static string Exists = "Funcionário já se encontra registrado!";
        public readonly static string NotFound = "Funcionário não encontrado!";
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
        public readonly static string SuccessCanceled = "Solicitação de registro de usuário cancelada com sucesso.";
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
}
