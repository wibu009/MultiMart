﻿namespace MultiMart.Infrastructure.Auth.OAuth2.Facebook;

public class Userinfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
}