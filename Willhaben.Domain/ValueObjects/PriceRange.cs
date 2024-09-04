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

    public bool IsValidPriceFrom(int priceFrom)
    {
        Console.WriteLine("tttttt");

        if (priceFrom < 0 | PriceFrom.HasValue)
        {
            Console.WriteLine("false");
            return false;
        }

        if (PriceTo.HasValue & PriceTo < priceFrom)
        {
            Console.WriteLine("false2");

            return false;
        }
        Console.WriteLine("true");

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

    public void SetAsGiveAway()
    {
        PriceTo = 0;
        PriceFrom = 0;
    }
}