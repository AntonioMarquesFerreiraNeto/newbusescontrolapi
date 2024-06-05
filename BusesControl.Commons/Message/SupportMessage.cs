namespace BusesControl.Commons.Message;

public class SupportMessage
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
}
