using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Utils;

namespace Willhaben.Domain.Models;
/*
public interface IUrlBuilder
{
    public string URL { get; }
}
public class WillhabenUrlBuilder: IUrlBuilder
{
    public static string BaseUrl { get; set; } =
        "https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz";
    public Guid SfId { get; set; }
    

    public List<Keyword> Keywords { get; private set; } = new();
    public List<State> States { get; private set; } = new();

    public List<Location> Locations { get; private set; } = new();
    
    public Seller Seller { get; private set; } = Seller.BOTH;
    public Handover Handover { get; private set; } = Handover.BOTH;

    public bool OnlyGiveAway { get; private set; } 
    public PriceRange PriceRange { get; set; } = new();
    public Category Category { get; set; } = new();
    
    public bool Paylivery { get; private set; } = false;

    private int _rows { get; set; } = 100;
    public int Rows
    {
        get => _rows;
        set
        {
            if (value > 0 & value <= 200)
            {
                _rows = value;
            }
            else
            {
                throw new InvalidRowCountException();
            }
        }
    }
    public string URL => $"{BaseUrl}" +
                         $"{(!string.IsNullOrEmpty(Category.SelectedCategory) ? $"/{Category.SelectedCategory}" : string.Empty)}" +
                         $"?sfId={SfId}" +
                         $"&rows={_rows}" + 
                         $"&isNavigation=true" + 
                         $"&keyword={string.Join("%20", Keywords.Select(keyword => keyword.ToString()))}" +
                         $"{(PriceRange.PriceFrom.HasValue ? $"&PRICE_FROM={PriceRange.PriceFrom.Value}" : string.Empty)}" +
                         $"{(PriceRange.PriceTo.HasValue ? $"&PRICE_TO={PriceRange.PriceTo.Value}" : string.Empty)}" + 
                         $"{string.Join(string.Empty, Locations.Select(location => $"&AreaId={location.GetValue()}"))}" +
                         $"{(Seller == Seller.BOTH ? string.Empty : $"&ISPRIVATE={Seller.GetValue()}")}" + 
                         $"{(Handover == Handover.BOTH ? string.Empty : $"&treeAttributes={Handover.GetValue()}")}" + 
                         $"{string.Join(string.Empty, States.Select(state => $"&treeAttributes={state.GetValue()}"))}" +
                         $"{(Paylivery ? $"&paylivery=true": String.Empty)}";
    
    public WillhabenUrlBuilder(Guid sfId = default)
    {
        SfId = sfId == default ? Guid.NewGuid() : sfId;
    }
    
    public WillhabenUrlBuilder OnlyGiveaway()
    {
        if (PriceRange.PriceFrom is not null || PriceRange.PriceTo is not null)
        {
            throw new PriceException("Cannot set giveaway with existing prices");
        }

        OnlyGiveAway = true;
        PriceRange.SetAsGiveAway();
        return this;
    }


    public WillhabenUrlBuilder AddLocation(Location location)
    {
        Locations.AddUnique(location);
        return this;
    }

    public WillhabenUrlBuilder AddState(State state)
    {
        States.AddUnique(state);
        return this;
    }
    
    public WillhabenUrlBuilder OnlyPaylivery()
    {
        if (Paylivery)
        {
            throw new PayliveryAlreadySetException();
        }
        Paylivery = true;
        return this; 
    }

    public WillhabenUrlBuilder OnlyPrivateSellers()
    {
        if (Seller == Seller.BOTH)
        {
            Seller = Seller.PRIVATE;
            return this;
        }
        throw new SellerAlreadySetException();
    }
    
    public WillhabenUrlBuilder OnlyCommercialSellers()
    {
        if (Seller == Seller.BOTH)
        {
            Seller = Seller.COMMERCIAL;
            return this;
        }
        throw new SellerAlreadySetException();
    }

    public WillhabenUrlBuilder OnlyShipment()
    {
        if (Handover == Handover.BOTH)
        {
            Handover = Handover.SHIPMENT;
            return this;
        }

        throw new HandoverAlreadySetException();
    }

    public WillhabenUrlBuilder OnlySelfCollection()
    {
        if (Handover == Handover.BOTH)
        {
            Handover = Handover.SELFCOLLECTION;
            return this;
        }

        throw new HandoverAlreadySetException();
    }

    public WillhabenUrlBuilder SetCategory(string category)
    {
        if (Category.SelectedCategory is not null)
        {
            throw new CategoryAlreadySetException();
        }
        Category.SelectedCategory = category;
        return this;
    }
    public WillhabenUrlBuilder SetPriceFrom(int priceFrom)
    {
        if (OnlyGiveAway | PriceRange.PriceFrom > 0)
        {
            throw new PriceAlreadySetException();
        }
        PriceRange.SetPriceFrom(priceFrom);
        return this;
    }

    public WillhabenUrlBuilder SetPriceTo(int priceTo)
    {
        if (OnlyGiveAway | PriceRange.PriceTo > 0)
        {
            throw new PriceAlreadySetException();
        }
        PriceRange.SetPriceTo(priceTo);
        return this;
    }
    
    public WillhabenUrlBuilder AddOmitKeyword(string keyword)
    {
        Keyword.AddUnique(Keywords,new OmitKeyword(keyword));
        return this;
    }

    public WillhabenUrlBuilder AddExactKeyword(string keyword)
    {
        Keyword.AddUnique(Keywords,new ExactKeyword(keyword));
        return this;
    }
    public WillhabenUrlBuilder AddKeyword(string keyword)
    {
        Keyword.AddUnique(Keywords,new FuzzyKeyword(keyword));
        return this;
    }
    
}
*/