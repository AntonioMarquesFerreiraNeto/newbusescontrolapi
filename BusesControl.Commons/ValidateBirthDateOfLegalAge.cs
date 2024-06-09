namespace BusesControl.Commons;

public static class ValidateBirthDateOfLegalAge
{
    public static bool IsValid(DateOnly birthDate)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18));
        if (birthDate >= date)
        {
            return false;
        }

        return true;
    }
}
