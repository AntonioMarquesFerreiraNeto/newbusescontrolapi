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
    public static string Key = "sua chave secreta da API para autenticação";
    public static int ExpireHours = 3;
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
