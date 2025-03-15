namespace BusesControl.Commons
{
    public class TemplatePayloadGenerative
    {
        public static string GetPayloadSimpleQuestion(string content) => @$"Seu nome é Buses Control Assistant, uma IA do sistema Buses Control. Responda objetivamente em JSON, onde cada parágrafo é um item de uma lista; se curto, apenas um item. Pergunta: ""{content}"". Formato esperado: [""Primeiro parágrafo."", ""Segundo parágrafo, se necessário.""]. Sem explicações, apenas JSON.";
    }
}
