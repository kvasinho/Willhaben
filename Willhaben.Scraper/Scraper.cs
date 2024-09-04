using Willhaben.Scraper.Products;

namespace Willhaben.Scraper;




public abstract class Scraper<TObject>
{
    //Scraper should just have a single scrape method that returns a collcetion of any object 
    //This should furthermore contain inputs like User Agents, proxies etc. 
    //Retries and timing are not managed in here but rather in the manager. 
     public abstract string Key { get; set; }
     
     public List<string> UserAgents { get; set; } = new List<string>
     {
         "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
         "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Safari/605.1.15",
     };
     
     protected readonly IJsonToUrlConverterService? JsonToUrlConverterService;

     public Scraper(IJsonToUrlConverterService? urlBuilderService = null)
     {
         this.JsonToUrlConverterService = urlBuilderService;
     }
     public abstract ScrapingResult<TObject> Scrape();
}
/*
public class WillhabenScraper : Scraper<WillhabenResult>
{
    //UrlbuilderService nutzen um die Url zu erstellen. 
    public override string Key { get; set; } = "willhaben";
    
    public WillhabenScraper(string filepath): base(new JsonWillhabenUrlService(filepath)){}

    public override ScrapingResult<WillhabenResult> Scrape()
    {
        var url = base.UrlBuilderService.Url;
        return new ScrapingResult<WillhabenResult>();
    }
}
*/
public class EbayScraper : Scraper<EbayResult>
{
    //UrlbuilderService nutzen um die Url zu erstellen. 
    public override string Key { get; set; } = "ebay";

    public override ScrapingResult<EbayResult> Scrape()
    {
        return new ScrapingResult<EbayResult>();
    }
}

//Wullhaben scraper -> implementation of Scraper with defined Scrape method. 

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
