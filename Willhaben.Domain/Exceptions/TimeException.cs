namespace Willhaben.Domain.Exceptions;

public class TimeException : Exception
{
    public TimeException(string message) : base(message) { }
}



public class EqualTimeException : TimeException
{
    public EqualTimeException() : base($"Times cannot be equal"){}
}