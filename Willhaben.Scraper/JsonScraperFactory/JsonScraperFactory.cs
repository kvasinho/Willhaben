using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper;
using Willhaben.Scraper.Implementations;

public static class JsonScraperFactory
{
    public static IScraperSettings CreateFromScraperType(ScraperType scraperType)
    {
        return scraperType switch
        {
            ScraperType.WILLHABEN => new WillhabenScraperSettings(),
            ScraperType.EBAY => new WillhabenScraperSettings(), // Modify for specific settings if needed
            _ => throw new NotSupportedException($"Scraper type {scraperType} is not supported")
        };
    }

    public static async Task<ICollection<IScraperSettings>> LoadSettingsFromDirectoryAsync(string dirname = @"Settings/Scrapers")
    {
        if (!Directory.Exists(dirname))
        {
            throw new DirectoryNotFoundException($"{dirname} does not exist");
        }
        
        var settings = new List<IScraperSettings>();
        foreach (var file in Directory.GetFiles(dirname))
        {
            if (file.EndsWith(".json"))
            {
                var setting = await CreateFromJsonAsync(file);
                settings.Add(setting);
            }
        }
        return settings;
    }

    public static async Task<List<IScraper>> LoadScrapersFromDirectoryAsync(string dirname = @"Settings/Scrapers")
    {
        if (!Directory.Exists(dirname))
        {
            throw new DirectoryNotFoundException($"{dirname} does not exist");
        }
        var scrapers = new List<IScraper>();
        foreach (var file in Directory.GetFiles(dirname))
        {

            if (file.EndsWith(".json"))
            {

                var settings = await CreateFromJsonAsync(file);

                // Create the scraper instance
                var scraper = CreateScraperFromSettings(settings);

                if (scraper != null)
                {
                    scrapers.Add(scraper);
                }
                else
                {
                    Console.WriteLine("Failed to create scraper");
                }
            }
        }
        return scrapers;
    }
        

    // Method to create scraper from settings and type
    private static IScraper CreateScraperFromSettings(IScraperSettings settings)
    {
        return settings switch
        {
            WillhabenScraperSettings willhabenScraperSettings =>
                new WillhabenScraper(willhabenScraperSettings),
            _ => throw new NotSupportedException($"Scraper settings type {settings.GetType()} is not supported")
        };
    }


    // Method to get the correct scraper type based on the ScraperType enum
    private static Type GetScraperType(ScraperType scraperType)
    {
        switch (scraperType)
        {
            case ScraperType.WILLHABEN:
                return typeof(WillhabenScraper);
            case ScraperType.EBAY:
                // Replace with actual eBay scraper type
                return typeof(WillhabenScraper); // Assuming you have an EbayScraper class
            default:
                throw new NotSupportedException($"Scraper type {scraperType} is not supported");
        }
    }

    public static async Task<IScraperSettings> CreateFromJsonAsync(string filePath)
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
