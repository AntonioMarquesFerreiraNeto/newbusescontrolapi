﻿namespace BusesControl.Entities.DTOs;

public class SendEmail
{
    public string Recipient { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string HtmlTemplate { get; set; } = default!;
}
