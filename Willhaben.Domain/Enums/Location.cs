using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Models;


public enum Location
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
    KREMS_AN_DER_DONAU = 301,
    SANKT_POELTEN = 302,
    WAIDHOFEN_AN_DER_YBBS= 303,
    WIENER_NEUSTADT = 304,
    AMSTETTEN = 305,
    BADEN = 306,
    BRUCK_AN_DER_LEITHA = 307,
    GAENSERDORF = 308,
    GMUEND = 309,
    HOLLABRUNN = 310,
    HORN = 311,
    KORNEUBURG = 312,
    KREMS_LAND = 313,
    LILIENFELD = 314,
    MELK = 315,
    MISTELBACH = 316,
    MOEDLING = 317,
    NEUNKIRCHEN = 318,
    SANKT_POELTEN_LAND = 319,
    SCHEIBBS = 320,
    TULLN = 321,
    WAIDHOFEN_AN_DER_THAYA = 322,
    WIENER_NEUSTADT_LAND = 323,
    ZWETTL = 325,
    
    OBEROESTERREICH = 4,
    LINZ = 401,
    STEYR = 402,
    WELS = 403,
    BRAUNAU = 404,
    EFERDING = 405,
    FREISTADT = 406,
    GMUNDEN = 407,
    GRIESKIRCHEN = 408,
    KIRCHDORF_AN_DER_KREMS = 409,
    LINZ_LAND =  410,
    PERG = 411,
    RIED = 412,
    ROHRBACH = 413,
    SCHAERDING = 414,
    STEYR_LAND = 415,
    URFAHR_UMGEBUNG = 416,
    VOECKLABRUCK = 417,
    WELS_LAND  = 418,
    
    
    SALZBURG = 5,
    SALZBURG_STADT = 501,
    HALLEIN = 502,
    SALZBURG_UMGEBUNG = 503,
    ST_JOHANN = 504,
    TAMSWEG = 505,
    ZELL_AM_SEE = 506,
    
    STEIERMARK = 6,
    GRAZ = 601,
    DEUTSCHLANDSBERG = 603,
    GRAZ_UMGEBUNG = 606,
    LEIBNITZ = 610,
    LEOBEN = 611,
    LIETZEN = 612,
    MURAU = 614,
    VOITSBERG = 616,
    WEITZ = 617,
    MURTAL = 620,
    BRUCK_MUERZZUSCHLAG = 621,
    HARTBERG_FUERSTENFELD = 622,
    SUEDOSTSTEIERMARK = 623,
    
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
    ALL = -1,
}
/*
public class LocationCollection
    {
        private readonly List<Location> _locations;

        public IReadOnlyList<Location> Locations => _locations.AsReadOnly();

        public IReadOnlyList<Location> SimplifiedLocations => SimplifyLocations();

        public LocationCollection()
        {
            _locations = new List<Location>();
        }

        public void AddUnique(Location location)
        {
            if (_locations.Contains(location))
            {
                throw new DuplicateKeyException<Location>(location);
            }
            _locations.Add(location);
        }

        private List<Location> SimplifyLocations()
        {

            var simplified = new List<Location>(_locations);


            if (HasAllWienDistricts(simplified))
            {
                simplified.RemoveAll(l => IsWienDistrict(l) || l == Location.WIEN);
                simplified.Add(Location.WIEN);
            }

            if (HasAllBurgenlandRegions(simplified))
            {
                simplified.RemoveAll(l => IsBurgenlandRegion(l) || l == Location.BURGENLAND);
                simplified.Add(Location.BURGENLAND);
            }

            if (HasAllTirolCities(simplified))
            {
                simplified.RemoveAll(l => IsTirolCity(l) || l == Location.TIROL);
                simplified.Add(Location.TIROL);
            }
            
            if (HasAllVoralbergCities(simplified))
            {
                simplified.RemoveAll(l => IsVoralbergCity(l) || l == Location.VORARLBERG);
                simplified.Add(Location.VORARLBERG);
            }
            
            if (HasAllSalzburgCities(simplified))
            {
                simplified.RemoveAll(l => IsSalzburgCity(l) || l == Location.SALZBURG);
                simplified.Add(Location.SALZBURG);
            }
            if (HasAllKaerntenCities(simplified))
            {
                simplified.RemoveAll(l => IsKaerntenCity(l) || l == Location.KAERNTEN);
                simplified.Add(Location.KAERNTEN);
            }
            if (HasAllNiederÖsterreichCities(simplified))
            {
                simplified.RemoveAll(l => IsNiederÖsterreichCity(l) || l == Location.NIEDEROESTERREICH);
                simplified.Add(Location.NIEDEROESTERREICH);
            }
            if (HasAllOberÖsterreichCities(simplified))
            {
                simplified.RemoveAll(l => IsOberÖsterreichCity(l) || l == Location.OBEROESTERREICH);
                simplified.Add(Location.OBEROESTERREICH);
            }
            if (HasAllSteiermarkCities(simplified))
            {
                simplified.RemoveAll(l => IsSteiermarkCity(l) || l == Location.STEIERMARK);
                simplified.Add(Location.STEIERMARK);
            }
            if (HasAllMainRegions(simplified))
            {
                simplified = new List<Location>();
            }
            return simplified;
        }
        
        private bool HasAllMainRegions(List<Location> locations)
        {
            var mainRegions = new[]
            {
                Location.WIEN,
                Location.BURGENLAND,
                Location.KAERNTEN,
                Location.NIEDEROESTERREICH,
                Location.OBEROESTERREICH,
                Location.SALZBURG,
                Location.STEIERMARK,
                Location.TIROL,
                Location.VORARLBERG
            };

            return mainRegions.All(locations.Contains);
        }
        private bool HasAllWienDistricts(List<Location> locations)
        {
            for (int i = 1; i <= 23; i++)
            {
                if (!locations.Contains((Location)Enum.Parse(typeof(Location), $"WIEN_{i:D2}")))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsWienDistrict(Location location)
        {
            return location >= Location.WIEN_01 && location <= Location.WIEN_23;
        }

        private bool HasAllKaerntenCities(List<Location> locations)
        {
            return locations.Contains(Location.KLAGENFURT) &&
                   locations.Contains(Location.VILLACH) &&
                   locations.Contains(Location.FELDKIRCHEN) &&
                   locations.Contains(Location.KLAGENFURT_LAND) &&
                   locations.Contains(Location.SPITTAL) &&
                   locations.Contains(Location.VILLACH_LAND) &&
                   locations.Contains(Location.VOELKERMARKT) &&
                   locations.Contains(Location.WOLFSBERG) &&
                   locations.Contains(Location.HERMAGOR) ||
                   locations.Contains(Location.KAERNTEN);
        }
        
        private bool IsKaerntenCity(Location location)
        {
            return location >= Location.KLAGENFURT && location <= Location.HERMAGOR;
        }
        
        private bool HasAllNiederÖsterreichCities(List<Location> locations)
        {
            return locations.Contains(Location.KREMS_AN_DER_DONAU) &&
                   locations.Contains(Location.SANKT_POELTEN) &&
                   locations.Contains(Location.WAIDHOFEN_AN_DER_YBBS) &&
                   locations.Contains(Location.WIENER_NEUSTADT) &&
                   locations.Contains(Location.AMSTETTEN) &&
                   locations.Contains(Location.BADEN) &&
                   locations.Contains(Location.BRUCK_AN_DER_LEITHA) &&
                   locations.Contains(Location.GAENSERDORF) &&
                   locations.Contains(Location.GMUEND) &&
                   locations.Contains(Location.HOLLABRUNN) &&
                   locations.Contains(Location.HORN) &&
                   locations.Contains(Location.KORNEUBURG) &&
                   locations.Contains(Location.KREMS_LAND) &&
                   locations.Contains(Location.LILIENFELD) &&
                   locations.Contains(Location.MELK) &&
                   locations.Contains(Location.MISTELBACH) &&
                   locations.Contains(Location.MOEDLING) &&
                   locations.Contains(Location.NEUNKIRCHEN) &&
                   locations.Contains(Location.SANKT_POELTEN_LAND) &&
                   locations.Contains(Location.SCHEIBBS) &&
                   locations.Contains(Location.TULLN) &&
                   locations.Contains(Location.WAIDHOFEN_AN_DER_THAYA) &&
                   locations.Contains(Location.WIENER_NEUSTADT_LAND) &&
                   locations.Contains(Location.ZWETTL) ||
                   locations.Contains(Location.NIEDEROESTERREICH);
        }
        
        private bool IsNiederÖsterreichCity(Location location)
        {
            return location >= Location.KREMS_AN_DER_DONAU && location <= Location.ZWETTL;
        }


        private bool HasAllSteiermarkCities(List<Location> locations)
        {
            return locations.Contains(Location.GRAZ) &&
                   locations.Contains(Location.DEUTSCHLANDSBERG) &&
                   locations.Contains(Location.GRAZ_UMGEBUNG) &&
                   locations.Contains(Location.LEIBNITZ) &&
                   locations.Contains(Location.LEOBEN) &&
                   locations.Contains(Location.LIETZEN) &&
                   locations.Contains(Location.MURAU) &&
                   locations.Contains(Location.VOITSBERG) &&
                   locations.Contains(Location.WEITZ) &&
                   locations.Contains(Location.MURTAL) &&
                   locations.Contains(Location.BRUCK_MUERZZUSCHLAG) &&
                   locations.Contains(Location.HARTBERG_FUERSTENFELD) &&
                   locations.Contains(Location.SUEDOSTSTEIERMARK) ||
                   locations.Contains(Location.STEIERMARK);
        }
        
        private bool IsSteiermarkCity(Location location)
        {
            return location >= Location.GRAZ && location <= Location.SUEDOSTSTEIERMARK;
        }

        private bool HasAllOberÖsterreichCities(List<Location> locations)
        {
            return locations.Contains(Location.LINZ) &&
                   locations.Contains(Location.STEYR) &&
                   locations.Contains(Location.WELS) &&
                   locations.Contains(Location.BRAUNAU) &&
                   locations.Contains(Location.EFERDING) &&
                   locations.Contains(Location.FREISTADT) &&
                   locations.Contains(Location.GMUNDEN) &&
                   locations.Contains(Location.GRIESKIRCHEN) &&
                   locations.Contains(Location.KIRCHDORF_AN_DER_KREMS) &&
                   locations.Contains(Location.LINZ_LAND) &&
                   locations.Contains(Location.PERG) &&
                   locations.Contains(Location.RIED) &&
                   locations.Contains(Location.ROHRBACH) &&
                   locations.Contains(Location.SCHAERDING) &&
                   locations.Contains(Location.STEYR_LAND) &&
                   locations.Contains(Location.URFAHR_UMGEBUNG) &&
                   locations.Contains(Location.VOECKLABRUCK) &&
                   locations.Contains(Location.WELS_LAND) ||
                   locations.Contains(Location.OBEROESTERREICH);
        }
        
        private bool IsOberÖsterreichCity(Location location)
        {
            return location >= Location.LINZ && location <= Location.WELS_LAND;
        }
        
        private bool HasAllBurgenlandRegions(List<Location> locations)
        {
            return locations.Contains(Location.EISENSTADT) &&
                   locations.Contains(Location.RUST) &&
                   locations.Contains(Location.EISENSTADT_UMGEBUNG) &&
                   locations.Contains(Location.GUESSING) &&
                   locations.Contains(Location.JENNERSDORF) &&
                   locations.Contains(Location.MATTERSBURG) &&
                   locations.Contains(Location.NEUSIEDL) &&
                   locations.Contains(Location.OBERPULLENDORF) &&
                   locations.Contains(Location.OBERWART) ||
                   locations.Contains(Location.BURGENLAND);
        }

        private bool IsBurgenlandRegion(Location location)
        {
            return location >= Location.EISENSTADT && location <= Location.OBERWART;
        }
        
        private bool HasAllTirolCities(List<Location> locations)
        {
            return locations.Contains(Location.INNSBRUCK) &&
                   locations.Contains(Location.IMST) &&
                   locations.Contains(Location.INNSBRUCK_LAND) &&
                   locations.Contains(Location.KITZBUEHEL) &&
                   locations.Contains(Location.KUFSTEIN) &&
                   locations.Contains(Location.LANDECK) &&
                   locations.Contains(Location.LIENZ) &&
                   locations.Contains(Location.REUTTE) &&
                   locations.Contains(Location.SCHWAZ) ||
                   locations.Contains(Location.TIROL);
        }

        private bool IsTirolCity(Location location)
        {
            return location >= Location.INNSBRUCK && location <= Location.SCHWAZ;
        }
        
        private bool HasAllVoralbergCities(List<Location> locations)
        {
            return locations.Contains(Location.BLUDENZ) &&
                   locations.Contains(Location.BREGENZ) &&
                   locations.Contains(Location.FELDKIRCH) &&
                   locations.Contains(Location.DORNBIRN) ||
                   locations.Contains(Location.VORARLBERG);
        }
        
        private bool IsVoralbergCity(Location location)
        {
            return location == Location.BLUDENZ ||
                   location == Location.BREGENZ ||
                   location == Location.DORNBIRN ||
                   location == Location.FELDKIRCH ||
                   location == Location.VORARLBERG;
        }
        
            
        private bool HasAllSalzburgCities(List<Location> locations)
        {
            return locations.Contains(Location.SALZBURG_STADT) &&
                   locations.Contains(Location.HALLEIN) &&
                   locations.Contains(Location.SALZBURG_UMGEBUNG) &&
                   locations.Contains(Location.ST_JOHANN) &&
                   locations.Contains(Location.TAMSWEG) &&
                   locations.Contains(Location.ZELL_AM_SEE) ||
                   locations.Contains(Location.SALZBURG);
        }
        
        private bool IsSalzburgCity(Location location)
        {
            return location >= Location.SALZBURG_STADT && location <= Location.ZELL_AM_SEE;
        }

        public static LocationCollection FromLocationsList(List<Location> locations)
        {
            LocationCollection collection = new LocationCollection();
            locations.ForEach(location =>
            {
                collection.AddUnique(location);
            });
            return collection;
        }
    }
    
    */