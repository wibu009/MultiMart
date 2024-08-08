﻿namespace MultiMart.Infrastructure.Auth.OAuth.Google;

public class GoogleSettings
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string CallBackPath { get; set; } = default!;
}