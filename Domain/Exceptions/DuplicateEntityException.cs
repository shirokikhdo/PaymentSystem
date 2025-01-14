namespace Domain.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string? message = null) 
        : base(message)
    {
        
    }
}