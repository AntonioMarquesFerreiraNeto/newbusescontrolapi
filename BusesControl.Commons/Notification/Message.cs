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
    }

    public class User
    {
        public readonly static string CredentialsInvalid = "E-mail ou senha informados podem estar incorretos!";
        public readonly static string Exists = "Usuário já se encontra registrado!";
        public readonly static string InvalidRole = "Permisão selecionada é inválida!";
        public readonly static string Unexpected = "Desculpe, não conseguimos registrar o usuário!";
    }
}
