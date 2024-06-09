using System.Text.RegularExpressions;

namespace BusesControl.Commons;

public static class OnlyNumbers
{
    public static string ClearValue(string value) 
    {
        return Regex.Replace(value, @"[^\d]", "");
    }
}
