using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper.Implementations;

namespace Willhaben.Scraper
{
    public interface IScraper
    {
        Key Key { get; set; }
        IScraperSettings ScraperSettings { get; set; }
        Task<object> ScrapeAsync();
    }
    

    /// <summary>
    /// Base class for scrapers with a specific result type.
    /// </summary>
    public interface  IScraper<T>: IScraper where T: class
    {
        //public Key Key { get; set; }
        //public IScraperSettings ScraperSettings { get; set; }
        
        /// <summary>
        /// Asynchronously performs the scraping operation and returns a result of type <typeparamref name="TResult"/>.
        /// </summary>
        Task<ScrapingResult<T>> ScrapeAsync();
    }
}