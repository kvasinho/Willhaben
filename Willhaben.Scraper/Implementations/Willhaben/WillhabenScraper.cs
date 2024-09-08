using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;


namespace Willhaben.Scraper.Implementations
{
    
    public class WillhabenScraper:  IScraper<WillhabenItem>, IScraper
    {
        public Key Key { get; set; } = new Key(ScraperType.WILLHABEN.ToString());
        public IScraperSettings ScraperSettings { get; set; }


        public WillhabenScraper(IScraperSettings scraperSettings)
        {
            ScraperSettings = scraperSettings;
        }
      
        public async Task<ScrapingResult<WillhabenItem>> ScrapeAsync()
        {
            var data = new List<WillhabenItem>();

            if (ScraperSettings.Url is null)
            {
                throw new Exception("Could not locate url");
            }

            if (ScraperSettings is WillhabenScraperSettings willhabenScraperSettings)
            {
                willhabenScraperSettings.SetSfId();
                
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                
                client.DefaultRequestHeaders.Add("authority", "www.willhaben.at");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("accept-language", "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7");
                client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");
                //client.DefaultRequestHeaders.Add("x-bbx-csrf-token", GenerateCSRFToken());
                client.DefaultRequestHeaders.Add("x-wh-client", "api@willhaben.at;responsive_web;server;1.0.0;desktop");


                
                HttpResponseMessage response = await client.GetAsync(willhabenScraperSettings.Url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    
                    // Parse the JSON response
                    using JsonDocument doc = JsonDocument.Parse(jsonString);
                    JsonElement root = doc.RootElement;

                    // Navigate to advertSummaryList and then advertSummary
                    if (root.TryGetProperty("advertSummaryList", out JsonElement advertSummaryList) &&
                        advertSummaryList.TryGetProperty("advertSummary", out JsonElement advertSummary))
                    {
                        // Iterate through each ad in advertSummary
                        foreach (JsonElement adElement in advertSummary.EnumerateArray())
                        {
                            try
                            {
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true,
                                    AllowTrailingCommas = true, // Not supported by System.Text.Json
                                    ReadCommentHandling = JsonCommentHandling.Skip // Allows comments in JSON (if needed)
                                };
                                WillhabenItem? item = JsonSerializer.Deserialize<WillhabenItem>(adElement.ToString(),options);
                                if (item is not null)
                                {
                                    Console.WriteLine(item.Description);
                                    data.Add(item);
                                }
                                else
                                {
                                    Console.WriteLine("item is null");
                                }
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"Error parsing ad: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new WillhabenResult(data, false, StatusCode.BAD_REQUEST, $"HTTP error: {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("NOT A WILLHABEN SETTING");
                return new WillhabenResult(data, false, StatusCode.BAD_REQUEST, "Invalid scraper settings");
            }
            //data.ForEach(item => Console.WriteLine(item));
            return new WillhabenResult(data, true, StatusCode.OK, null);
        }
        

        async Task<object> IScraper.ScrapeAsync()
        {
            var result = await ScrapeAsync();
            return result; // Return the ScrapingResult<WillhabenScrapingResult> as an object
        }
        public WillhabenScraper(WillhabenScraperSettings settings)
        {
            ScraperSettings = settings;
            Key = settings.Key;
        }
        
        
    }
}