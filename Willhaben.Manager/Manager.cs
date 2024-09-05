using Microsoft.Extensions.Logging;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper;

namespace MyConsoleApp;

public class Manager
{
    private readonly ILogger _logger;

    private readonly Dictionary<Key, List<Scraper>> _scrapers = new();
    private readonly Dictionary<Key, PriorityQueue<Scraper, DateTime>> _queues =
        new Dictionary<Key, PriorityQueue<Scraper, DateTime>>();
    
    private void AddScraper(Scraper scraper, DateTime dateTime)
    {
        _queues[scraper.Key].Enqueue(scraper, dateTime);
        if (!_scrapers.TryGetValue(scraper.Key, out var scraperList))
        {
            scraperList = new List<Scraper>();
            _scrapers[scraper.Key] = scraperList;
        }
        scraperList.Add(scraper);
    }
    //Calculate the scraper times and add to queues
    private void CreateQueues()
    {
        foreach (var key in _scrapers.Keys)
        {
            //
        }
    }
    
    
    //Read all scrapers -> creates a list of scrapers. 
    
    
    
}