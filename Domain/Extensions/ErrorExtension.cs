namespace Domain.Extensions;

public static class ErrorExtension
{
    public static string ToText(this Exception exception) =>
        $"{exception.Message} {exception.StackTrace} {exception.InnerException?.Message} {exception.InnerException?.StackTrace}";
}