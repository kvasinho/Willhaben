using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;

namespace Willhaben.Scraper.Implementations.Willhaben;

public class WillhabenScraper : SerializableScraperBase
{
    public WillhabenScraper(WillhabenScraperSettings settings)
        : base(settings)
    {
    }
    
    public override async Task<ScrapingResult<TResult>> Scrape<TResult>()
    {
        await Task.Delay(1);
        return new ScrapingResult<TResult>();    
    }
}