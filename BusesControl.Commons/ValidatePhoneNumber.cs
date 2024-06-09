using System.Text.RegularExpressions;

namespace BusesControl.Commons;

public static class ValidatePhoneNumber
{
    public static bool IsValid(string phoneNumber)
    {
        phoneNumber = OnlyNumbers.ClearValue(phoneNumber);
        if (phoneNumber.Length != 11)
        {
            return false;
        }

        return true;
    }
}
