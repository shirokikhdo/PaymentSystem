namespace Domain.Exceptions;

public class SoftEntityNotFoundException : Exception
{
    public SoftEntityNotFoundException(string message)
        : base(message)
    {

    }
}