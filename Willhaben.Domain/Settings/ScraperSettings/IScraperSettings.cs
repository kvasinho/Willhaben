using Willhaben.Domain.Models;

namespace Willhaben.Domain.Settings;

/// <summary>
/// This Is the Settings interface for all Scrapers.
/// Every scraper implementation needs an Id, some SchedulingSettings and a key.
/// Key is unique to scrapers of a specific type, ID is unique to a single scraper.
/// </summary>
public interface IScraperSettings
{
    public Guid Id { get; set; }
    public ScraperType ScraperType { get; }
    public ScrapingScheduleSettings ScrapingScheduleSettings { get; set; }
}
public interface ISerializableScraperSettings : IScraperSettings
{
    string Url { get; }
    Task ToJsonAsync();
}

/// <summary>
/// This Is the Interface for a json serializable scraper. (A scraper with all settings stored in json).
/// It needs to implement from and to json methods and contain a Url (for transparency reasons).
/// </summary>
public interface ISerializableScraperSettings<T>: ISerializableScraperSettings
{
    public static abstract Task<T> FromJsonAsync(string filePath);
}
