using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper;
using Willhaben.Scraper.Implementations.Willhaben;

public static class JsonScraperFactory
{
    
    public static ISerializableScraperSettings CreateFromScraperType(ScraperType scraperType)
    {
        return scraperType switch
        {
            ScraperType.WILLHABEN => new WillhabenScraperSettings(),
            ScraperType.EBAY => new WillhabenScraperSettings(),
            _ => throw new NotSupportedException($"Scraper type {scraperType} is not supported")
        };
    }
    

    public static async Task<ICollection<ISerializableScraperSettings>> LoadSettingsFromDirectoryAsync(
        string dirname = @$"Settings/Scrapers")
    {
        if (!Directory.Exists(dirname))
        {
            throw new DirectoryNotFoundException($"{dirname} does not exist");
        }
        var settings = new List<ISerializableScraperSettings>();
        foreach (var file in Directory.GetFiles(dirname))
        {
            if (file.EndsWith(".json"))
            {
                var setting = await CreateFromJsonAsync(Path.Join(file));
                settings.Add(setting);
            }
        }
        return settings;
    }
    public static async Task<List<SerializableScraperBase>> LoadScrapersFromDirectoryAsync(
        string dirname = @$"Settings/Scrapers")
    {
        if (!Directory.Exists(dirname))
        {
            throw new DirectoryNotFoundException($"{dirname} does not exist");
        }
        var scrapers = new List<SerializableScraperBase>();
        foreach (var file in Directory.GetFiles(dirname))
        {
            if (file.EndsWith(".json"))
            {
                var setting = await CreateFromJsonAsync(Path.Join(file));
                var scraper = CreateFromSettings(setting);
                scrapers.Add(scraper);
            }
        }
        return scrapers;
    }

    public static SerializableScraperBase CreateFromSettings(
        ISerializableScraperSettings settings)
    {
        return settings switch
        {
           WillhabenScraperSettings willhabenScraperSettings => new WillhabenScraper(willhabenScraperSettings),
            _ => throw new NotSupportedException($"Scraper settings type {settings.GetType()} is not supported")
        };
    }

    
    public static async Task<ISerializableScraperSettings> CreateFromJsonAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exist", filePath);
        }

        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            var document = await JsonDocument.ParseAsync(fileStream);
            var root = document.RootElement;

            if (!root.TryGetProperty("ScraperType", out var scraperTypeElement))
            {
                throw new JsonException("The JSON file does not contain a 'ScraperType' property.");
            }

            var scraperType = (ScraperType)scraperTypeElement.GetInt32();

            return scraperType switch
            {
                ScraperType.WILLHABEN => await WillhabenScraperSettings.FromJsonAsync(filePath),
                ScraperType.EBAY => await WillhabenScraperSettings.FromJsonAsync(filePath),
                _ => throw new NotSupportedException($"Scraper type {scraperType} is not supported")
            };
        }
    }
}