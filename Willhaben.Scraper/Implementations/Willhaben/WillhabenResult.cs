using System.Globalization;
using System.Text.Json.Serialization;
using Willhaben.Domain.Models;

namespace Willhaben.Scraper.Implementations;

public class WillhabenResult : ScrapingResult<WillhabenItem>
{
    public WillhabenResult(ICollection<WillhabenItem> data, bool success, StatusCode statusCode,
        string? errorMessage) : base(ScraperType.WILLHABEN, data, success, statusCode, errorMessage){}

}


public class WillhabenItem
{
    public string Id { get; set; }
    public int VerticalId { get; set; }
    public int AdTypeId { get; set; }
    public int ProductId { get; set; }
    
    //public AdvertStatus AdvertStatus { get; set; }
    public string Description { get; set; }

    [JsonIgnore]
    public Attributes Attributes
    {
        get
        {
            return Attributes;
        }
        set
        {
            if (value is not null)
            {
                Attributes = value;
                TranslatedAttributes = TranslatedAttributes.FromAttributesArray(value.Attribute);
            }
        }
    }

    public AdvertImageList AdvertImageList { get; set; }
    public string SelfLink { get; set; }

    public TranslatedAttributes TranslatedAttributes { get; set; } = new TranslatedAttributes();
    

    public override string ToString()
    {
        string imageUrls = string.Join(", ", AdvertImageList.AdvertImage.Select(image => image.MainImageUrl));
        TranslatedAttributes.FromAttributesArray(Attributes.Attribute);

        return $"Id: {Id}, " +
               $"VerticalId: {VerticalId}, " +
               $"AdTypeId: {AdTypeId}, " +
               $"ProductId: {ProductId}, " +
               $"Description: {Description}, " +
               $"SelfLink: {SelfLink}";
    }
}

public class AdvertStatus
{
    public string Id { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
}

public class Attributes
{
    public List<AttributeObject> Attribute { get; set; }
}

public record Coordinates(decimal Latitude, decimal Longitude);
public class TranslatedAttributes
{
    public string City { get; set; } //LOCATION string 
    public int PostCode { get; set; } 
    public string Body { get; set; }
    public string Heading { get; set; }
    public string Country { get; set; }
    public Location Region { get; set; } //State string
    public Location Location { get; set; } //DISTRICT
    public string Address { get; set; }
    public DateTime StartDate { get; set; } //PUBLISHED_String  
    public float Price { get; set; } //"PRICE
    public DateTime EndDate { get; set; } //ENDDATE_String
    public string SeoUrl { get; set; } //SEO_URL
    public List<string> ImageUrls { get; set; } = new List<string>(); //ALL_IMAGE_URLS
    public Seller Seller { get; set; } //ISPRIVATE
    public Coordinates Coordinates { get; set; } //COORDINATES array
    

    public static TranslatedAttributes FromAttributesArray(List<AttributeObject> attributes)
    {
        TranslatedAttributes attrs = new TranslatedAttributes();
        attrs.Region = AttributeObject.ExtractValueFromName<Location>(attributes, "STATE");
        attrs.Location = AttributeObject.ExtractValueFromName<Location>(attributes, "DISTRICT");
        attrs.PostCode = AttributeObject.ExtractValueFromName<int>(attributes, "POSTCODE");
        attrs.Body = AttributeObject.ExtractValueFromName<string>(attributes, "BODY_DYN");
        attrs.Heading = AttributeObject.ExtractValueFromName<string>(attributes, "HEADING");
        
        attrs.Country = AttributeObject.ExtractValueFromName<string>(attributes, "COUNTRY");
        attrs.EndDate = AttributeObject.ExtractValueFromName<DateTime>(attributes, "ENDDATE_String");
        attrs.StartDate  = AttributeObject.ExtractValueFromName<DateTime>(attributes, "PUBLISHED_String");
        attrs.Seller = AttributeObject.ExtractValueFromName<Seller>(attributes, "ISPRIVATE");
        attrs.Address  = AttributeObject.ExtractValueFromName<string>(attributes, "ADDRESS");
        attrs.Coordinates  = AttributeObject.ExtractCoordinatesValueFromName(attributes, "COORDINATES");
        attrs.Price = AttributeObject.ExtractPriceValueFromName(attributes, "PRICE");
        attrs.SeoUrl  = AttributeObject.ExtractValueFromName<string>(attributes, "SEO_URL");
        
        return attrs;


    }
}


public class AttributeObject
{
    public string Name { get; set; }
    public string[] Values { get; set; }

    public static Coordinates ExtractCoordinatesValueFromName(List<AttributeObject> attributeObjects, string name)
    {
        var attribute = attributeObjects.FirstOrDefault(attr => attr.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (attribute != null && attribute.Values.Length > 0)
        {
            string value = attribute.Values[0]; // e.g., "48.2028,16.41967"

            try
            {
                var parts = value.Split(',');

                if (parts.Length == 2 &&
                    decimal.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal latitude) &&
                    decimal.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal longitude))
                {
                    return new Coordinates(latitude, longitude);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting coordinates: {ex.Message}");
            }
        }

        return new Coordinates(0, 0);
    }

    public static T ExtractValueFromName<T>(List<AttributeObject> attributeObjects, string name)
    {
        var attribute = attributeObjects.FirstOrDefault(attr => attr.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (attribute != null && attribute.Values.Length > 0)
        {
            string value = attribute.Values[0];

            try
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value.Replace(" ", "_").Replace("-", "_"), true);
                }

                if (typeof(T) == typeof(float))
                {
                    if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                    {
                        return (T)(object)result;
                    }
                }

                if (typeof(T) == typeof(double))
                {
                    if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                    {
                        return (T)(object)result;
                    }
                }

                if (typeof(T) == typeof(decimal))
                {
                    if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                    {
                        return (T)(object)result;
                    }
                }

                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting value: {ex.Message}");
                return default;
            }
        }

        return default;
    }
    public static float ExtractPriceValueFromName(List<AttributeObject> attributeObjects, string name)
    {
        var attribute = attributeObjects.FirstOrDefault(attr => attr.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(attribute.Values[0]);

        if (attribute is null || attribute.Values.Length == 0)
        {
            Console.WriteLine("Error parsing price");
            return default;
        }

        string value = attribute.Values[0];
        

        // Ensure the input string is not null or empty
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Price string cannot be null or empty", nameof(value));
        }

        try
        {
            // Try parsing the string to a float using InvariantCulture
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Error parsing price: {ex.Message}");
            throw; // Rethrow or handle as needed
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"Price value is too large or too small: {ex.Message}");
            throw; // Rethrow or handle as needed
        }
    }


}

public class AdvertImageList
{
    public List<AdvertImage> AdvertImage { get; set; }
    //public List<object> FloorPlans { get; set; }
}

public class AdvertImage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SelfLink { get; set; }
    public string Description { get; set; }
    public string MainImageUrl { get; set; }
    public string ThumbnailImageUrl { get; set; }
    public string ReferenceImageUrl { get; set; }
    public object SimilarImageSearchUrl { get; set; }
    public string Reference { get; set; }
}

public class ContextLinkList
{
    public List<ContextLink> ContextLink { get; set; }
}

public class ContextLink
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string Uri { get; set; }
    public bool Selected { get; set; }
    public string RelativePath { get; set; }
    public string ServiceName { get; set; }
}

public class AdvertiserInfo
{
    public object Label { get; set; }
    public object IconSVG { get; set; }
    public object IconPNG { get; set; }
    public string IconType { get; set; }
}