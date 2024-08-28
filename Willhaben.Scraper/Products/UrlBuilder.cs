using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Scraper.Products;

public class UrlBuilder
{
    public string BaseUrl { get; set; } =
        "https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz";
    public Guid SfId { get; set; }

    public List<Keyword> Keywords { get; private set; } = new List<Keyword>();

    public List<Location> Locations { get; private set; } = new List<Location>();

    public PriceRange PriceRange { get; set; } = new PriceRange();
    public Category Category { get; set; } = new Category();

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
                         $"{(PriceRange.PriceTo.HasValue ? $"&PRICE_TO={PriceRange.PriceTo.Value}" : string.Empty)}";
                         

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
        PriceRange.SetPriceFrom(priceFrom);
        return this;
    }

    public UrlBuilder SetPriceTo(int priceTo)
    {
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