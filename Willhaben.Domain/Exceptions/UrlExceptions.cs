namespace Willhaben.Domain.Exceptions
{
    public class UrlException : Exception
    {
        public UrlException(string message) : base(message) { }
    }

    public class SellerAlreadySetException : UrlException
    {
        private const string DefaultMessage = "The seller type cannot be set twice";

        public SellerAlreadySetException() : base(DefaultMessage) { }
    }
    public class HandoverAlreadySetException : UrlException
    {
        private const string DefaultMessage = "The handover type cannot be set twice";

        public HandoverAlreadySetException() : base(DefaultMessage) { }
    }
    public class PayliveryAlreadySetException : UrlException
    {
        private const string DefaultMessage = "The paylivery type cannot be set twice";

        public PayliveryAlreadySetException() : base(DefaultMessage) { }
    }
    public class CategoryAlreadySetException : UrlException
    {
        private const string DefaultMessage = "The category  cannot be set twice";

        public CategoryAlreadySetException() : base(DefaultMessage) { }
    }

    public class InvalidRowCountException : UrlException
    {
        private const string DefaultMessage = "Rows must be between 1 and 200";

        public InvalidRowCountException() : base(DefaultMessage) { }
    }
    
    public class PriceAlreadySetException : UrlException
    {
        private const string DefaultMessage = "The price cannot be set twice";

        public PriceAlreadySetException() : base(DefaultMessage) { }
    }
}