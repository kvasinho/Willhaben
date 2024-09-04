using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Domain.Utils;
using System.Text.Json;

namespace Willhaben.Scraper.Products
{
    public interface IJsonToUrlConverterService
    {
        //Alle Converter nehmen einen UrlBuilder als Service mit rein. 
        //Alle Converter nehmen einen FilePath rein und geben eine Url aus.
        //Alle Converter haben eine bestimmte Config file als input -> sollte im namen spezifiziert werden
        //Was macht er? 
        //1. Lädt die File, 2. Validiert 3. Converted, 
        string FilePath { get; set; }
        string Url { get; }
        
        bool Validate();
        Task ParseAsync();
        
    }

    public class JsonEbayUrlService
    {
        
    }

    public class JsonWillhabenUrlService : IJsonToUrlConverterService
    {
        public  string FilePath { get; set; }
        public string Url => _willhabenUrlBuilder.URL;
        private readonly IJsonConfigFactory _jsonConfigFactory;

        private readonly WillhabenUrlBuilder _willhabenUrlBuilder;

        
        public JsonWillhabenUrlService(WillhabenUrlBuilder willhabenUrlBuilder, IJsonConfigFactory jsonConfigFactory, string filePath)
        {
            _willhabenUrlBuilder = willhabenUrlBuilder;
            _jsonConfigFactory = jsonConfigFactory;
            FilePath = filePath;
        }

        public bool Validate()
        {
            return true;
        }

        public async Task ParseAsync()
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var JsonConfig = await _jsonConfigFactory.CreateAsync(FilePath);
                
                if (JsonConfig is WillhabenJsonConfig willhabenConfig)
                {
                    Console.WriteLine($"Interval: {willhabenConfig.Settings.Interval}");
                    Console.WriteLine($"Days: {string.Join(", ", willhabenConfig.Settings.Days)}");
                    Console.WriteLine($"From: {willhabenConfig.Settings.From}");
                    Console.WriteLine($"To: {willhabenConfig.Settings.To}");
                    Console.WriteLine($"Type: {willhabenConfig.Settings.Type}");
                    Console.WriteLine($"Fuzzy Keywords: {string.Join(", ", willhabenConfig.Keywords.FuzzyKeywords)}");
                    Console.WriteLine($"Exact Keywords: {string.Join(", ", willhabenConfig.Keywords.ExactKeywords)}");
                    Console.WriteLine($"Omit Keywords: {string.Join(", ", willhabenConfig.Keywords.OmitKeywords)}");
                }
                else
                {
                    Console.WriteLine("Unexpected config type");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while parsing the file: {ex.Message}");
                // Optionally rethrow or handle the exception
            }
        }

        public void Convert()
        {
            throw new NotImplementedException();
        }
    }
}