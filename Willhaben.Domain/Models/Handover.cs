using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class Handover: AbstractCodedEnumBaseClass<HandoverType>
{
    public override HandoverType Value { get; set; }
    
    public Handover(HandoverType value = HandoverType.BOTH) : base(value)
    {
    }

    protected override Exception KeyNotFoundException(HandoverType value)
    {
        return new HandoverException($"There is no key for {value}");
    }
}

public enum HandoverType
{
    SELFCOLLECTION = 2536,
    SHIPMENT = 2537,
    BOTH = -1, 
}