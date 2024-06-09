﻿namespace BusesControl.Entities.Response;

public class LoginResponse
{
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }

    public LoginResponse(string token, DateTime expires)
    {
        Token = token;
        Expires = expires;
    }
}
