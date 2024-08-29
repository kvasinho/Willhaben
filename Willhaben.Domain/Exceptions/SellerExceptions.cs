
namespace Willhaben.Domain.Exceptions
{
    public class SellerException : Exception
    {
        public SellerException(string message) : base(message) { }
    }

    public class SellerAlreadySetExceptioon(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Method of Merchant was already set";

        public SellerAlreadySetExceptioon() : this(DefaultMessage) { }
    }
}