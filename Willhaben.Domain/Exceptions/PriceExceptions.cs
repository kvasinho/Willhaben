
namespace Willhaben.Domain.Exceptions
{
    public class PriceException : Exception
    {
        public PriceException(string message) : base(message) { }
    }

    public class NegativePriceException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Price cannot be negative";

        public NegativePriceException() : this(DefaultMessage) { }
    }
    public class MaximumPriceLowerThanMinimumPriceException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Maximum Price cannot be lower than minimum price";

        public MaximumPriceLowerThanMinimumPriceException() : this(DefaultMessage) { }
    }
}