﻿public class AppSettings
{
    public AppSettingsRedis Redis { get; set; } = default!;
    public AppSettingsViaCep ViaCep { get; set; } = default!;
    public AppSettingsJWT JWT { get; set; } = default!;
    public AppSettingsPdfCo PdfCo { get; set; } = default!;
    public AppSettingsAssas Assas { get; set; } = default!;
    public AppSettingsWebhookAssas WebhookAssas { get; set; } = default!;
    public AppSettingsEmail Email { get; set; } = default!;
    public AppSettingsResetPassword ResetPassword { get; set; } = default!;
    public AppSettingsSecurityCode SecurityCode { get; set; } = default!;
    public AppSettingsTermination Termination { get; set; } = default!;
    public AppSettingsUserSystem UserSystem { get; set; } = default!;
    public AppSettingsGoogleGemini GoogleGemini { get; set; } = default!;
}

public class AppSettingsRedis 
{
    public int Expire { get; set; }
    public string Host { get; set; } = default!;
}

public class AppSettingsViaCep
{
    public string Url { get; set; } = default!;
}

public class AppSettingsJWT
{
    public string Key { get; set; } = default!;
    public int ExpireHours { get; set; }
}

public class AppSettingsPdfCo
{
    public string Key { get; set; } = default!;
    public string Url { get; set; } = default!;
}

public class AppSettingsAssas
{
    public string Key { get; set; } = default!;
    public string Url { get; set; } = default!;
}

public class AppSettingsWebhookAssas
{
    public string SendType { get; set; } = default!;
    public bool Enabled { get; set; }
    public bool Interrupted { get; set; }
    public int ApiVersion { get; set; }
}

public class AppSettingsEmail
{
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Host { get; set; } = default!;
    public string Key { get; set; } = default!;
    public int Port { get; set; }
}

public class AppSettingsResetPassword
{
    public int ExpireCode { get; set; }
    public int ExpireResetToken { get; set; }
    public int CodeLength { get; set; }
}

public class AppSettingsSecurityCode
{
    public int ExpireCode { get; set; }
    public int CodeLength { get; set; }
}

public class AppSettingsTermination
{
    public int AddTerminateDays { get; set; }
}

public class AppSettingsUserSystem
{
    public string Role { get; set; } = default!;
}

public class AppSettingsGoogleGemini 
{ 
    public string ApiToken { get; set; } = default!;
    public string BaseUrl { get; set; } = default!;
}