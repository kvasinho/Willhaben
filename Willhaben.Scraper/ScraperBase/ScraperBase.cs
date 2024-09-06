using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;

namespace Willhaben.Scraper;

public abstract class ScraperBase
{
    //public abstract string Id { get; set; } <- IS IN SCRAPERSETTINGS
    public  Key Key { get; set; } 
    
    public abstract  Task<ScrapingResult<TScrapingResult>> Scrape<TScrapingResult>()  where TScrapingResult: class;

    public IScraperSettings ScraperSettings { get; set; }

    public DateTime CalculateNextRun(DateTime? lastRun)
    {
        DateTime nextTime = lastRun ?? DateTime.Now;
        if (lastRun is not null)
        {
            nextTime.AddMinutes((double)ScraperSettings.ScrapingScheduleSettings.Interval);
        }

        return nextTime;
    }

    public ScraperBase(Key key)
    {
        Key = key;
    }
}

public abstract class SerializableScraperBase: ScraperBase
{
    //FromJson Class
    
    public ISerializableScraperSettings ScraperSettings { get; set; }

    protected SerializableScraperBase( ISerializableScraperSettings scraperSettings) : base(new Key(scraperSettings.ScraperType.ToString()))
    {
        ScraperSettings = scraperSettings;
    }

    public static async Task<SerializableScraperBase> FromJsonAsync(
        string filePath, 
        Func<ISerializableScraperSettings, SerializableScraperBase> createScraper)
    {
        // Deserialize the settings from the JSON file
        var settings = await JsonScraperFactory.CreateFromJsonAsync(filePath);

        // Use the factory delegate to create the scraper
        return createScraper(settings);
    }
    
}