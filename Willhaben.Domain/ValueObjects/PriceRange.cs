using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class PriceRange
{
    public int? PriceFrom { get;  set; }
    public int? PriceTo { get;  set; }

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

    public bool IsValidPriceFrom(int priceFrom)
    {

        if (priceFrom < 0 | PriceFrom.HasValue)
        {
            return false;
        }

        if (PriceTo.HasValue & PriceTo < priceFrom)
        {

            return false;
        }

        return true;
    }

    public bool IsValidPriceTo(int priceTo)
    {
        if (priceTo < 0 | PriceTo is not null)
        {
            return false;
        }

        if (PriceFrom.HasValue & PriceFrom > priceTo)
        {
            return false;
        }

        return true;
    }
    
}