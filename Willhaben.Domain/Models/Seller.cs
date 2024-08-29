using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class Seller: AbstractCodedEnumBaseClass<SellerType>
{
    
    public override SellerType Value { get; set; }
    
    public Seller(SellerType value = SellerType.BOTH) : base(value)
    {
    }

    protected override Exception KeyNotFoundException(SellerType value)
    {
        return new SellerException($"There is no key for {value}");
    }
}

public enum SellerType
{
    PRIVATE = 1,
    COMMERCIAL = 0,
    BOTH = -1
}