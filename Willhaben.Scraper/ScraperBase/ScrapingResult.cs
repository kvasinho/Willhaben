using Willhaben.Domain.Models;

namespace Willhaben.Scraper;

public class ScrapingResult<T>
{
    public ICollection<T> Data { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public StatusCode StatusCode { get; set; }
}
