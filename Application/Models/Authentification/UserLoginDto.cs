﻿namespace Application.Models.Authentification;

public class UserLoginDto
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Password { get; set; }
}