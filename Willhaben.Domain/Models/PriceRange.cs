using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class PriceRange
{
    public int? PriceFrom { get; private set; }
    public int? PriceTo { get; private set; }

    public void SetPriceFrom(int priceFrom)
    {
        if (priceFrom < 0)
            throw new NegativePriceException();

        if (PriceTo.HasValue && priceFrom > PriceTo)
            throw new MaximumPriceLowerThanMinimumPriceException();

        PriceFrom = priceFrom;
    }

    public void SetPriceTo(int priceTo)
    {
        if (priceTo < 0)
            throw new NegativePriceException();

        if (PriceFrom.HasValue && PriceFrom > priceTo)
            throw new MaximumPriceLowerThanMinimumPriceException();

        PriceTo = priceTo;
    }

    public void SetAsGiveAway()
    {
        PriceTo = 0;
        PriceFrom = 0;
    }
}