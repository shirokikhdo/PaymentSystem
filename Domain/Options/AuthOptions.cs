using System.ComponentModel.DataAnnotations;

namespace Domain.Options;

public class AuthOptions
{
    public string TokenPrivateKey { get; set; }

    public int ExpireIntervalMinutes { get; set; }
}