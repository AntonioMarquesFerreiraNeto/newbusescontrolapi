using System.Text;
using System.Text.Json;

namespace BusesControl.Commons
{
    public class GenerateBase64
    {
        public static string Generate<T>(T input)
        {
            var bytes = input is string ? Encoding.UTF8.GetBytes(input as string) : Encoding.UTF8.GetBytes(JsonSerializer.Serialize(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
