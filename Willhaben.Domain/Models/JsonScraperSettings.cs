using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Utils;
using Willhaben.Domain.Utils.Converters;

namespace Willhaben.Domain.Models;

public interface IJsonSettings
{
    public string Url { get; }
    [JsonIgnore]
    public string Filename { get; }
    public ScraperType ScraperType { get; }
         
    public IScraperSettings ScraperSettings { get; set; }

}
public class WillhabenJsonSettings: IJsonSettings
    {
        [JsonIgnore]
        public static string BaseUrl { get; set; } =
            "https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz";
        [JsonIgnore]
        public Guid SfId { get; set; } = Guid.NewGuid();
        
        public string Url
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

                var locations = string.Join(string.Empty, Locations.SimplifiedValues.Select(location => $"&AreaId={location.GetValue()}"));
                var seller = Seller == Seller.BOTH ? string.Empty : $"&ISPRIVATE={Seller.GetValue()}";
                var handover = Handover == Handover.BOTH ? string.Empty : $"&treeAttributes={Handover.GetValue()}";
                var states = string.Join(string.Empty, States.SimplifiedValues.Select(state => $"&treeAttributes={state.GetValue()}"));
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
        
        [JsonIgnore]
        public string Filename
        {
            get
            {
                
                if (FuzzyKeywords.Any() || ExactKeywords.Any() || OmitKeywords.Any())
                {
                    // Build parts for each type of keyword
                    var fuzzyPart = FuzzyKeywords.Any() ? string.Join("_", FuzzyKeywords.Select(kw => kw.Value)) : null;
                    var exactPart = ExactKeywords.Any() ? string.Join("_", ExactKeywords.Select(kw => kw.Value)) : null;
                    var omitPart = OmitKeywords.Any() ? string.Join("_", OmitKeywords.Select(kw => kw.Value)) : null;

                    // Combine the parts with "_" if they exist
                    var parts = new List<string>();

                    if (fuzzyPart != null) parts.Add(fuzzyPart);
                    if (exactPart != null) parts.Add(exactPart);
                    if (omitPart != null) parts.Add(omitPart);

                    // Join the parts with "__" to separate different keyword classes
                    return string.Join("_", parts).GenerateRandomNDigitString();
                }

                return $"no_keywords_".GenerateRandomNDigitString();
                
            }
        }
        
        public ScraperType ScraperType { get; } = ScraperType.WILLHABEN;
        
        [JsonConverter(typeof(KeywordListJsonConverter<FuzzyKeyword>))]
        public List<FuzzyKeyword> FuzzyKeywords { get; set; } = new List<FuzzyKeyword>();
        
        [JsonConverter(typeof(KeywordListJsonConverter<ExactKeyword>))]
        public List<ExactKeyword> ExactKeywords { get; set; } = new List<ExactKeyword>();
        
        [JsonConverter(typeof(KeywordListJsonConverter<OmitKeyword>))]
        public List<OmitKeyword> OmitKeywords { get; set; } = new List<OmitKeyword>();

        [JsonIgnore]
        public List<Keyword> Keywords =>
            FuzzyKeywords.Cast<Keyword>()
                .Concat(ExactKeywords.Cast<Keyword>())
                .Concat(OmitKeywords.Cast<Keyword>())
                .ToList();
        
        public bool AsPresentOnly { get; set; } = false;
        public bool PayliveryOnly { get; set; } = false;
        public PriceRange PriceRange { get; set; } = new PriceRange();
        public LocationCollection Locations { get; set; } = new LocationCollection();
        public StateCollection States { get; set; } = new StateCollection();
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

        public IScraperSettings ScraperSettings { get; set; } = new ScraperSettings();
        
        public static async Task<WillhabenJsonSettings> FromJsonAsync(string filePath)
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
                var settings = await JsonSerializer.DeserializeAsync<WillhabenJsonSettings>(fileStream);
                return settings;
            }
        }   
        
    }
