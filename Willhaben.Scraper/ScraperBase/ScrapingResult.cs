using Willhaben.Domain.Models;

namespace Willhaben.Scraper.Implementations;


public interface IScrapingResult
{
    public ScraperType ScraperType { get; }
    int Count { get; }
    bool Success { get; }
    string? ErrorMessage { get; }
    StatusCode StatusCode { get; }
    string ToString();
}
public class ScrapingResult<T>: IScrapingResult where T: class
{
    public ScraperType ScraperType { get; set; }
    public ICollection<T> Data { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get;  }
    public StatusCode StatusCode { get; set; }

    public ScrapingResult(ScraperType scraperType, ICollection<T> data, bool success, StatusCode statusCode, string? errorMessage)
    {
        ScraperType = scraperType;
        Data = data;
        Success = success;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
    
    public int Count => Data.Count;
    public override string ToString()
    {
        return $"Success: {Success}, Results: {Data.Count} ";
    }
}
