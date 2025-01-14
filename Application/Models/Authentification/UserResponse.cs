﻿namespace Application.Models.Authentification;

public class UserResponse
{
    public long Id { get; set; }

    public string[]? Roles { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    public string? Phone { get; set; }

    public string? Token { get; set; }
}