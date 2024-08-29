
namespace Willhaben.Domain.Exceptions
{
    public class HandoverException : Exception
    {
        public HandoverException(string message) : base(message) { }
    }

    public class HandoverAlreadySetExceptioon(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Method of handover was already set";

        public HandoverAlreadySetExceptioon() : this(DefaultMessage) { }
    }
}