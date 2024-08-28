using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models
{
    public class Location
    {
        private string _value;

        public int Code
        {
            get
            {
                if (string.IsNullOrEmpty(_value) || !Locations.TryGetValue(_value, out int code))
                {
                    throw new InvalidLocationException($"Invalid location: {_value ?? "null"}");
                }
                return code;
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrEmpty(value) || !Locations.ContainsKey(value))
                {
                    throw new InvalidLocationException($"Invalid location: {value ?? "null"}");
                }
                _value = value;
            }
        }

        public Location(string value)
        {
            Value = value; // This will use the setter and validate the input
        }

        public static Dictionary<string, int> Locations = new Dictionary<string, int>
        {
            { "burgenland", 1 },
                {"eisenstadt", 101},
                {"rust", 102},
                {"eisenstadt-umgebung", 103},
                {"güssing", 104},
                {"jennersdorf", 105},
                {"mattersburg", 106},
                {"neusiedl", 107},
                {"oberpullendorf", 108},
                {"oberwart", 109},
            { "kärnten", 2 },
                {"klagenfurt", 201},
                {"villach", 202},
                {"feldkirchen", 203},
                {"klagenfurt-land", 204},
                {"sankt-veit", 205},
                {"spittal", 206},
                {"villach-land", 207},
                {"völkermarkt", 208},
                {"wolfsberg", 209},
                {"hermagor", 210},
            { "niederösterreich", 3 },
            { "oberösterreich", 4 },
            { "salzburg", 5 },
                {"salzburg-stadt", 501},
                {"hallein", 502},
                {"salzburg-umgebung", 503},
                {"st-johann", 504},
                {"tamsweg", 505},
                {"zell-am-see", 506},
            { "steiermark", 6 },
            { "tirol", 7 },
                {"innsbruck", 701},
                {"imst", 702},
                {"innsbruck-land", 703},
                {"kitzbühel", 704},
                {"kufstein", 705},
                {"landeck", 706},
                {"lienz", 707},
                {"reutte", 708},
                {"schwaz", 709},
            { "vorarlberg", 8 },
                { "bludenz", 801 },
                { "bregenz", 802 },
                { "dornbirn", 803 },
                { "feldkirch", 804 },
            { "wien", 900 },
                { "01", 117223 },
                { "02", 117224 },
                { "03", 117225 },
                { "04", 117226 },                
                { "05", 117227 },
                { "06", 117228 },                
                { "07", 117229 },
                { "08", 117230 },                
                { "09", 117231 },
                { "10", 117232 },
                { "11", 117233 },
                { "12", 117234 },
                { "13", 117235 },
                { "14", 117236 },
                { "15", 117237 },
                { "16", 117238 },
                { "17", 117239 },
                { "18", 117240 },
                { "19", 117241 },
                { "20", 117242 },
                { "21", 117243 },
                { "22", 117244 },
                { "23", 117245 },
            { "andere", 22000 }
            
        };
    }

}