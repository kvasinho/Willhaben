using Willhaben.Domain.Models;

namespace Willhaben.Domain.Settings;

public class DummyScraperSettings: ISerializableScraperSettings<WillhabenScraperSettings>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ScraperType ScraperType { get; } = ScraperType.CUSTOM;
    public ScrapingScheduleSettings ScrapingScheduleSettings { get; set; }
    public string Url => "www.test.de";
    public static Task<WillhabenScraperSettings> FromJsonAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task ToJsonAsync()
    {
        throw new NotImplementedException();
    }
}