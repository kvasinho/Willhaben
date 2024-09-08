using System.Text.Json.Serialization;
using Willhaben.Domain.Models;
using Willhaben.Domain.StronglyTypedIds;

namespace Willhaben.Domain.Settings;

/// <summary>
/// This Is the Settings interface for all Scrapers.
/// Every scraper implementation needs an Id, some SchedulingSettings and a key.
/// Key is unique to scrapers of a specific type, ID is unique to a single scraper.
/// </summary>
public interface IScraperSettings
{
    public Guid Id { get; set; }
    public Key Key { get; set; }
    public ScraperType ScraperType { get; }
    public ScrapingScheduleSettings ScrapingScheduleSettings { get; set; }
    
    [JsonIgnore]
    string? Url { get; }
}

