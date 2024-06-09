namespace BusesControl.Commons;

public static class AppSettingsViaCep
{
    public static string Url = "https://viacep.com.br/ws";
}

public static class AppSettingsJWT
{
    public static string Key = "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yqsST1peCXBNyeCXBNy4DpeCXBNy4DjCb3cFFHs4dnL04y";
    public static int ExpireHours = 4;
}

public static class AppSettingsResetPassword
{
    public static int ExpireCode = 3;
    public static int ExpireResetToken = 15;    
}