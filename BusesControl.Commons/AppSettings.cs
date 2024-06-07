namespace BusesControl.Commons;

public class AppSettings
{
    public AppSettingsViaCep ViaCep { get; set; } = new AppSettingsViaCep();
    public AppSettingsJWT JWT { get; set; } = new AppSettingsJWT();
}

public class AppSettingsViaCep
{
    public string Url = "https://viacep.com.br/ws";
}

public class AppSettingsJWT
{
    public string Key = "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yqsST1peCXBNyeCXBNy4DpeCXBNy4DjCb3cFFHs4dnL04y";
    public int ExpireHours = 4;
}
