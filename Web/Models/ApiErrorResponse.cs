namespace Web.Models;

public class ApiErrorResponse
{
    public int Code { get; set; }

    public string Message { get; set; } = null!;

    public string? Description { get; set; }
}