using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Models
{
    public class Location: AbstractCodedEnumBaseClass<LocationType>
    {

        public override LocationType Value { get; set; }
        
        public Location(LocationType value = LocationType.ALL) : base(value)
        {
        }
        
        protected override Exception KeyNotFoundException(LocationType value)
        {
            throw new InvalidLocationException(value);
        }
        
        public static void AddLocation(ICollection<Location> locations, LocationType location)
        {
            
            if (locations == null)
                throw new ArgumentNullException(nameof(location));

            if (location == null)
                throw new ArgumentNullException(nameof(location));
            
            if (location == LocationType.ALL)
            {
                throw new InvalidLocationException(location);
            }

            if (locations.Any(l => l.Value == location))
            {
                throw new LocationExistsException(location);
            }
            locations.Add(new Location(location));
        }
        public static void AddLocation(ICollection<Location> locations, Location location)
        {
            if (locations == null)
                throw new ArgumentNullException(nameof(location));

            if (location == null)
                throw new ArgumentNullException(nameof(location));
            
            if (location.Value == LocationType.ALL)
            {
                throw new InvalidLocationException(location.Value);
            }
            if (locations.Any(l => l.Value == location.Value))
            {
                throw new LocationExistsException(location.Value);
            }
            locations.Add(location);
        }

        public static void AddLocations(ICollection<Location> locations, ICollection<LocationType> toAdd)
        {
            
            foreach (var location in toAdd)
            {
                AddLocation(locations,location);
            }
        }
        public static void AddLocations(ICollection<Location> locations, ICollection<Location> toAdd)
        {
            foreach (var location in toAdd)
            {
                AddLocation(locations,location);
            }
        }
    }
}

public enum LocationType
{
    BURGENLAND = 1,
    EISENSTADT = 101,
    RUST = 102,
    EISENSTADT_UMGEBUNG = 103,
    GUESSING = 104,
    JENNERSDORF = 105,
    MATTERSBURG = 106,
    NEUSIEDL = 107,
    OBERPULLENDORF = 108,
    OBERWART = 109,
    KAERNTEN = 2,
    KLAGENFURT = 201,
    VILLACH = 202,
    FELDKIRCHEN = 203,
    KLAGENFURT_LAND = 204,
    SANKT_VEIT = 205,
    SPITTAL = 206,
    VILLACH_LAND = 207,
    VOELKERMARKT = 208,
    WOLFSBERG = 209,
    HERMAGOR = 210,
    NIEDEROESTERREICH = 3,
    OBEROESTERREICH = 4,
    SALZBURG = 5,
    SALZBURG_STADT = 501,
    HALLEIN = 502,
    SALZBURG_UMGEBUNG = 503,
    ST_JOHANN = 504,
    TAMSWEG = 505,
    ZELL_AM_SEE = 506,
    STEIERMARK = 6,
    TIROL = 7,
    INNSBRUCK = 701,
    IMST = 702,
    INNSBRUCK_LAND = 703,
    KITZBUEHEL = 704,
    KUFSTEIN = 705,
    LANDECK = 706,
    LIENZ = 707,
    REUTTE = 708,
    SCHWAZ = 709,
    VORARLBERG = 8,
    BLUDENZ = 801,
    BREGENZ = 802,
    DORNBIRN = 803,
    FELDKIRCH = 804,
    WIEN = 900,
    WIEN_01 = 117223,
    WIEN_02 = 117224,
    WIEN_03 = 117225,
    WIEN_04 = 117226,
    WIEN_05 = 117227,
    WIEN_06 = 117228,
    WIEN_07 = 117229,
    WIEN_08 = 117230,
    WIEN_09 = 117231,
    WIEN_10 = 117232,
    WIEN_11 = 117233,
    WIEN_12 = 117234,
    WIEN_13 = 117235,
    WIEN_14 = 117236,
    WIEN_15 = 117237,
    WIEN_16 = 117238,
    WIEN_17 = 117239,
    WIEN_18 = 117240,
    WIEN_19 = 117241,
    WIEN_20 = 117242,
    WIEN_21 = 117243,
    WIEN_22 = 117244,
    WIEN_23 = 117245,
    ANDERE = 22000,
    ALL = -1
}

