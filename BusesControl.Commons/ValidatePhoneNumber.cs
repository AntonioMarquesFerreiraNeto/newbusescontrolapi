using System.Text.RegularExpressions;

namespace BusesControl.Commons;

public static class ValidatePhoneNumber
{
    public static bool IsValid(string phoneNumber)
    {
        phoneNumber = Regex.Replace(phoneNumber, @"[^a-z]", "");
        if (phoneNumber.Length != 11)
        {
            return false;
        }

        return true;
    }
}
