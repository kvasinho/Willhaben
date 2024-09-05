using Willhaben.Domain.Models;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper.Products;

namespace Willhaben.Scraper;

public abstract class Scraper
{
     public abstract Key Key { get; set; }
     
     public List<string> UserAgents { get; set; } = new List<string>
     {
         "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
         "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Safari/605.1.15",
     };
     
     public abstract  Task<ICollection<ScrapingResult<TObject>>> Scrape<TObject>();
     
     public ScraperSettings ScraperSettings { get; set; }

     public DateTime CalculateNextRun(DateTime? lastRun)
     {
         DateTime nextTime = lastRun ?? DateTime.Now;
         //if there was a last run, we want to make sure we are intervals after + within boundaries 
         
         //if there was no last run, we want to get first date within boundaries. 
         return nextTime;
     }
}
//Er kriegt die Settings direkt, weil sie davor eingelesen werden müssen. -> Class wird erstellt durch Factory Reading Method die nach Type separiert.
//SCraper wird damit instantgesetzt. 
//PriorityQueue wird für jeden Key erstellt -> dann wird gescraped. 
public class WillhabenScraper : Scraper
{
    private readonly WillhabenJsonSettings _willhabenJsonSettings;
    public WillhabenScraper(WillhabenJsonSettings willhabenJsonSettings)
    {
        _willhabenJsonSettings = willhabenJsonSettings;
    }

    public override Key Key { get; set; } =new Key(ScraperType.WILLHABEN.ToString());
    public override async Task<ICollection<ScrapingResult<WillhabenResult>>> Scrape<WillhabenResult>()
    {
        await Task.Delay(1);
        return new List<ScrapingResult<WillhabenResult>>();
    }

    public static async Task<WillhabenScraper> FromJson(string filePath)
    {
        var settings = await WillhabenJsonSettings.FromJsonAsync(filePath);
        return new WillhabenScraper(settings);
    }
}

public class EbayScraper : Scraper
{
    public override Key Key { get; set; } =new Key(ScraperType.WILLHABEN.ToString());

    public override async  Task<ICollection<ScrapingResult<EbayResult>>> Scrape<EbayResult>()
    {
        await Task.Delay(1);
        return new List<ScrapingResult<EbayResult>>();    
    }
}


public class ScrapingResult<T>
{
    public ICollection<T> Data { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int StatusCode { get; set; }
}
public class WillhabenResult
{
    public string Test { get; set; } = "willhaben";
}
public class EbayResult
{
    public string Test { get; set; } = "ebay";
}
public class CustomResult
{
    public string Test { get; set; } = "custom";
}
