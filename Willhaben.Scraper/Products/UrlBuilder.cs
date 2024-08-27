using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Scraper.Products;

public class UrlBuilder
{
    public string BaseUrl { get; set; }

    public bool IsEmptyKeywordSearch { get; set; } = false;
    public List<Keyword> Keywords { get; private set; }

    public PriceRange PriceRange { get; set; } = new PriceRange();

    public string Url => $"{BaseUrl}" +
                         $"&keyword={Keyword.DisplayKeywordList(Keywords)}" +
                         $"{(PriceRange.PriceFrom.HasValue ? $"&PRICE_FROM={PriceRange.PriceFrom.Value}" : string.Empty)}" +
                         $"{(PriceRange.PriceTo.HasValue ? $"&PRICE_TO={PriceRange.PriceTo.Value}" : string.Empty)}";

    public UrlBuilder(string baseUrl = "iad/search/atz/seo/kaufen-und-verkaufen/marktplatz?isNavigation=true&isISRL=true&srcType=vertical-search-box")
    {
        BaseUrl = baseUrl;
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
    
    public UrlBuilder OmitKeyword(string keyword)
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