namespace BusesControl.Commons;

public class AppSettings
{
    public AppSettingsViaCep ViaCep { get; set; } = new AppSettingsViaCep();
}

public class AppSettingsViaCep
{
    public string Url = "https://viacep.com.br/ws";
}
