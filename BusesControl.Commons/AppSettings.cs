namespace BusesControl.Commons;

//OBS: Por ser um sistema desenvolvido para compartilhar conhecimento, tais informações ficaram neste local para simplificar o processo.
//OBS: ...Mas se este sistema fosse usado por um cliente, tais informações ficariam em um azure key vault da vida
//OBS: ...Já que chaves de integrações e outras informações que podem ou não estar aqui devem ser sigilosas para a boa segurança do sistema.

public static class AppSettingsViaCep
{
    public static string Url = "https://viacep.com.br/ws";
}

public static class AppSettingsJWT
{
    public static string Key = "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yqsST1peCXBNyeCXBNy4DpeCXBNy4DjCb3cFFHs4dnL04y";
    public static int ExpireHours = 24;
}

public static class AppSettingsPdfCo 
{
    public static string Key = "sua chave da API";
    public static string Url = "https://api.pdf.co/v1/pdf/convert/from/html";
}

public static class AppSettingsAssas
{
    public static string Key = "sua chave da API";
    public static string Url = "https://sandbox.asaas.com/api/v3";
}

public static class AppSettingsEmail
{
    public static string UserName = "busescontrolmensagens@gmail.com";
    public static string Name = "Buses Control";
    public static string Host = "smtp.gmail.com";
    public static string KeyPassword = "sua senha de aplicativo";
    public static int Port = 587;
}

public static class AppSettingsResetPassword
{
    public static int ExpireCode = 3;
    public static int ExpireResetToken = 15;
    public static int CodeLenght = 10;
}

public static class AppSettingsSecurityCode 
{
    public static int ExpireCode = 3;
    public static int CodeLenght = 15;
}
