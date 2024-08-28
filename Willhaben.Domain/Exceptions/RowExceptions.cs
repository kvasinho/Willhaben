
namespace Willhaben.Domain.Exceptions
{
    public class RowException : Exception
    {
        public RowException(string message) : base(message) { }
    }

    public class InvalidRowCountException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Rows must be between 1 and 200";

        public InvalidRowCountException() : this(DefaultMessage) { }
    }
}