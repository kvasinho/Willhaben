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
    public Seller Seller { get; private set; } = Seller.BOTH;

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
                         $"{(Seller == Seller.BOTH ? string.Empty : Seller == Seller.PRIVATE ? "&ISPRIVATE=1": "&ISPRIVATE=0")}" + 
                         $"{string.Join(string.Empty, States.Select(state => $"&treeAttributes={state.Code}"))}";
    
    public UrlBuilder(Guid sfId = default)
    {
        SfId = sfId == default ? Guid.NewGuid() : sfId;
    }
    
    /*
    public UrlBuilder UseEmptyKeywordSearch()
    {
        if (Keywords.Count > 0)
        {
            throw new KeywordCollectionException();
        }
        IsEmptyKeywordSearch = true;
        return this;
    }
    */

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

    public UrlBuilder AddState(State.Zustand state)
    {
        State.AddZustand(States, state);
        return this;
    }
    public UrlBuilder SetSeller(Seller seller)
    {
        Seller = seller;
        return this;
    }
    public UrlBuilder SetSfId(Guid sfId)
    {
        SfId = sfId;
        return this; 
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

public enum Seller
{
    PRIVATE,
    MERCHANT,
    BOTH
}