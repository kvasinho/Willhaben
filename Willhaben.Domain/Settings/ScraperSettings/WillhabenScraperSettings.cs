using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Domain.Utils;
using Willhaben.Domain.Utils.Converters;

namespace Willhaben.Domain.Settings;

public class WillhabenScraperSettings: IScraperSettings
{
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonIgnore] public Key Key { get; set; } = new Key(ScraperType.WILLHABEN.ToString());

        [JsonIgnore] public static string BaseUrl { get; set; } =
            "https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz";
        [JsonIgnore] public Guid SfId { get; set; } = Guid.NewGuid();

        public void SetSfId()
        {
            SfId = Guid.NewGuid();
        }
        
        [JsonIgnore] public string Url
        {
            get
            {
                var baseUrl = $"{BaseUrl}" +
                              //$"{(!string.IsNullOrEmpty(Category.SelectedCategory) ? $"/{Category.SelectedCategory}" : string.Empty)}" +
                              $"?sfId={SfId}" +
                              $"&rows={_rows}" +
                              $"&isNavigation=true" +
                              $"&keyword={string.Join("%20", Keywords.Select(keyword => keyword.ToString()))}";

                var priceRange = AsPresentOnly 
                    ? $"&PRICE_FROM=0&PRICE_TO=0"
                    : $"{(PriceRange.PriceFrom > 0 ? $"&PRICE_FROM={PriceRange.PriceFrom.Value}" : string.Empty)}" +
                      $"{(PriceRange.PriceTo > 0 ? $"&PRICE_TO={PriceRange.PriceTo.Value}" : string.Empty)}";

                var locations = string.Join(string.Empty, Locations.SimplifiedValues.Select(location => $"&AreaId={(int)location}"));
                var seller = Seller == Seller.BOTH ? string.Empty : $"&ISPRIVATE={(int)Seller}";
                var handover = Handover == Handover.BOTH ? string.Empty : $"&treeAttributes={(int)Handover}";
                var states = string.Join(string.Empty, States.SimplifiedValues.Select(state => $"&treeAttributes={(int)state}"));
                var paylivery = PayliveryOnly ? "&paylivery=true" : string.Empty;

                return baseUrl +
                       priceRange +
                       locations +
                       seller +
                       handover +
                       states +
                       paylivery;
            }
        }
        
        public ScraperType ScraperType { get; } = ScraperType.WILLHABEN;
        
        [JsonConverter(typeof(KeywordListJsonConverter<FuzzyKeyword>))]
        public List<FuzzyKeyword> FuzzyKeywords { get; set; } = new List<FuzzyKeyword>();
        
        [JsonConverter(typeof(KeywordListJsonConverter<ExactKeyword>))]
        public List<ExactKeyword> ExactKeywords { get; set; } = new List<ExactKeyword>();
        
        [JsonConverter(typeof(KeywordListJsonConverter<OmitKeyword>))]
        public List<OmitKeyword> OmitKeywords { get; set; } = new List<OmitKeyword>();
        
        private bool KeywordExists(string keyword)
        {
            return FuzzyKeywords.Any(k => k.Value.Equals(keyword, StringComparison.OrdinalIgnoreCase)) ||
                   ExactKeywords.Any(k => k.Value.Equals(keyword, StringComparison.OrdinalIgnoreCase)) ||
                   OmitKeywords.Any(k => k.Value.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }
        public void AddFuzzyKeyword(FuzzyKeyword keyword)
        {
            if (KeywordExists(keyword.Value))
            {
                throw new ArgumentException($"The keyword '{keyword.Value}' already exists in one of the lists.");
            }

            FuzzyKeywords.Add(keyword);
        }
        public void AddFuzzyKeywords(List<FuzzyKeyword> keywords)
        {
            foreach (var keyword in keywords)
            {
                AddFuzzyKeyword(keyword);
            }
        }

        // Custom method to add exact keywords
        public void AddExactKeyword(ExactKeyword keyword)
        {
            if (KeywordExists(keyword.Value))
            {
                throw new ArgumentException($"The keyword '{keyword.Value}' already exists in one of the lists.");
            }

            ExactKeywords.Add(keyword);
        }
        public void AddExactKeywords(List<ExactKeyword> keywords)
        {
            foreach (var keyword in keywords)
            {
                AddExactKeyword(keyword);
            }
        }


        // Custom method to add omit keywords
        public void AddOmitKeyword(OmitKeyword keyword)
        {
            if (KeywordExists(keyword.Value))
            {
                throw new ArgumentException($"The keyword '{keyword.Value}' already exists in one of the lists.");
            }

            OmitKeywords.Add(keyword);
        }
        
        public void AddOmitKeywords(List<OmitKeyword> keywords)
        {
            foreach (var keyword in keywords)
            {
                AddOmitKeyword(keyword);
            }
        }
        

        [JsonIgnore]
        public List<Keyword> Keywords =>
            FuzzyKeywords.Cast<Keyword>()
                .Concat(ExactKeywords.Cast<Keyword>())
                .Concat(OmitKeywords.Cast<Keyword>())
                .ToList();
        
        public bool AsPresentOnly { get; set; } = false;
        public bool PayliveryOnly { get; set; } = false;
        
        [JsonConverter(typeof(PriceRangeConverter))]
        public PriceRange PriceRange { get; set; } = new ();
        [JsonConverter(typeof(SimplifyableCollectionConverter<LocationCollection,Location>))]
        public LocationCollection Locations { get; set; } = new ();
        
        [JsonConverter(typeof(SimplifyableCollectionConverter<StateCollection,State>))]
        public StateCollection States { get; set; } = new ();
        public Seller Seller { get; set; } = Seller.BOTH;
        public Handover Handover { get; set; } = Handover.BOTH;

        private int _rows { get; set; } = 30;
        public int Rows
        {
            get => _rows;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new InvalidRowCountException();
                }

                _rows = value;
            }
        }
        public ScrapingScheduleSettings ScrapingScheduleSettings { get; set; } = new();
        
        public static async Task<WillhabenScraperSettings> FromJsonAsync(string filePath)
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
                var settings = await JsonSerializer.DeserializeAsync<WillhabenScraperSettings>(fileStream);

                return settings;
            }
        }

        public async Task ToJsonAsync()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var test = JsonSerializer.Serialize(this, options);
    
            await using FileStream createStream = File.Create(@$"Settings/Scrapers/{Id}.json");
    
            await JsonSerializer.SerializeAsync(createStream, this, options);
    
            await createStream.FlushAsync();
        }
        
    }
