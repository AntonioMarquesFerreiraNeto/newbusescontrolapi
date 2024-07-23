namespace BusesControl.Commons;

public static class RegexString
{
    public static string PasswordPattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[\W_]).*$";
    public static string UrlPattern = @"^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$";
}
