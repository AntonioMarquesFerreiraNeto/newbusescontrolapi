namespace BusesControl.Commons;

public static class ValidateOpenDate
{
    public static bool IsValid(DateOnly value)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        if (value > dateNow)
        {
            return false;
        }

        return true;
    }
}
