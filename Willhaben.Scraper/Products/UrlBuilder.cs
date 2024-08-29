using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Scraper.Products;

public class UrlBuilder
{
    public string BaseUrl { get; set; } =
        "https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz";
    public Guid SfId { get; set; }
    

    public List<Keyword> Keywords { get; private set; } = new List<Keyword>();
    public List<State> States { get; private set; } = new List<State>();

    public List<Location> Locations { get; private set; } = new List<Location>();

    public bool OnlyGiveAway { get; private set; } 
    public PriceRange PriceRange { get; set; } = new PriceRange();
    public Category Category { get; set; } = new Category();
    
    public bool OnlyPrivateAds { get; set; }
    public Seller Seller { get; private set; }
    public Handover Handover { get; private set; }
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

    public string Url => $"{BaseUrl}" +
                         $"{(!string.IsNullOrEmpty(Category.SelectedCategory) ? $"/{Category.SelectedCategory}" : string.Empty)}" +
                         $"?sfId={SfId}" +
                         $"&rows={_rows}" + 
                         $"&isNavigation=true" + 
                         $"&keyword={Keyword.DisplayKeywordList(Keywords)}" +
                         $"{(PriceRange.PriceFrom.HasValue ? $"&PRICE_FROM={PriceRange.PriceFrom.Value}" : string.Empty)}" +
                         $"{(PriceRange.PriceTo.HasValue ? $"&PRICE_TO={PriceRange.PriceTo.Value}" : string.Empty)}" + 
                         $"{string.Join(string.Empty, Locations.Select(location => $"&AreaId={location.Code}"))}" +
                         $"{(Seller.Value == SellerType.BOTH ? string.Empty : $"&ISPRIVATE={Seller.Code}")}" + 
                         $"{(Handover.Value == HandoverType.BOTH ? string.Empty : $"&treeAttributes={Handover.Code}")}" + 
                         $"{string.Join(string.Empty, States.Select(state => $"&treeAttributes={state.Code}"))}" +
                         $"{(Paylivery ? $"paylivery=true": String.Empty)}";
    
    public UrlBuilder(Guid sfId = default)
    {
        SfId = sfId == default ? Guid.NewGuid() : sfId;
    }
    

    public UrlBuilder OnlyGiveAways()
    {
        if (PriceRange.PriceFrom is not null || PriceRange.PriceTo is not null)
        {
            throw new PriceException("Cannot set giveaway with existing prices");
        }

        OnlyGiveAway = true;
        PriceRange.SetAsGiveAway();
        return this;
    }

    public UrlBuilder AddState(StateType state)
    {
        State.AddZustand(States, state);
        return this;
    }
    public UrlBuilder SetSeller(Seller seller)
    {
        Seller = seller;
        return this;
    }

    public UrlBuilder OnlyPaylivery()
    {
        Paylivery = true;
        return this; 
    }

    public UrlBuilder OnlyPrivateSellers()
    {
        if (Seller.Value == SellerType.BOTH)
        {
            Seller.Value = SellerType.PRIVATE;
            return this;
        }
        throw new SellerAlreadySetExceptioon();
    }
    public UrlBuilder OnlyCommercialSellers()
    {
        if (Seller.Value == SellerType.BOTH)
        {
            Seller.Value = SellerType.COMMERCIAL;
            return this;
        }
        throw new SellerAlreadySetExceptioon();
    }

    public UrlBuilder OnlyShipment()
    {
        if (Handover.Value == HandoverType.BOTH)
        {
            Handover.Value = HandoverType.SHIPMENT;
            return this;
        }

        throw new HandoverAlreadySetExceptioon();
    }

    public UrlBuilder OnlySelfCollection()
    {
        if (Handover.Value == HandoverType.BOTH)
        {
            Handover.Value = HandoverType.SELFCOLLECTION;
            return this;
        }

        throw new HandoverAlreadySetExceptioon();
    }

    public UrlBuilder SetCategory(string category)
    {
        Category.SelectedCategory = category;
        return this;
    }
    public UrlBuilder SetPriceFrom(int priceFrom)
    {
        if (OnlyGiveAway)
        {
            throw new PriceException("Cannot set Price if query is marked as giveaway");
        }
        PriceRange.SetPriceFrom(priceFrom);
        return this;
    }

    public UrlBuilder SetPriceTo(int priceTo)
    {
        if (OnlyGiveAway)
        {
            throw new PriceException("Cannot set Price if query is marked as giveaway");
        }
        PriceRange.SetPriceTo(priceTo);
        return this;
    }

    public UrlBuilder SetAsPresentOnly()
    {
        PriceRange.SetPriceTo(0);
        PriceRange.SetPriceFrom(0);

        return this;
    }
    
    public UrlBuilder AddOmitKeyword(string keyword)
    {
        Keywords.Add(new Keyword(keyword, true));
        return this;
    }

    public UrlBuilder AddExactKeyword(string keyword)
    {
        Keywords.Add(new Keyword(keyword, false, true));
        return this;
    }
    public UrlBuilder AddKeyword(string keyword)
    {
        Keywords.Add(new Keyword(keyword));
        return this;
    }
    
}

