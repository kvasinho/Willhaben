using System.ComponentModel;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Willhaben.Domain.Utils;
using Willhaben.Domain.Utils.Converters;

namespace Willhaben.Presentation.Commands;

public class CreateCommand : Command<CreateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-s|--scraper")]
        public ScraperType? Type { get; set; }

        public List<FuzzyKeyword> _fuzzyKeywords;
        [CommandOption("--fuzzy <KEYWORDS>")]
        public string[] FuzzyKeywords
        {
            get => _fuzzyKeywords?.Select(k => k.Value).ToArray() ?? new string[]{};
            set
            {
                try
                {
                    _fuzzyKeywords = value.Select(v => new FuzzyKeyword(v)).ToList();
                }
                catch (KeywordException ex)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
                    _fuzzyKeywords = null;
                }
            }
        }

        public List<ExactKeyword> _exactKeywords;
        [CommandOption("--exact <KEYWORDS>")]
        public string[] ExactKeywords
        {
            get => _exactKeywords?.Select(k => k.Value).ToArray() ?? new string[]{};
            set
            {
                try
                {
                    _exactKeywords = value.Select(v => new ExactKeyword(v)).ToList();
                }
                catch (KeywordException ex)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
                    _exactKeywords = null;
                }
            }
        }

        public List<OmitKeyword> _omitKeywords;
        [CommandOption("--omit <KEYWORDS>")]
        public string[] OmitKeywords
        {
            get => _omitKeywords?.Select(k => k.Value).ToArray() ?? new string[]{};
            set
            {
                try
                {
                    _omitKeywords = value.Select(v => new OmitKeyword(v)).ToList();
                }
                catch (KeywordException ex)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
                    _omitKeywords = null;
                }
            }
        }
        [CommandOption("-p|--present-only")]
        public bool? AsPresentOnly { get; set; }
        
        [CommandOption("--paylivery-only")]
        public bool? PayliveryOnly { get; set; }

        public PriceRange PriceRange { get; set; } = new PriceRange();
        [CommandOption("--price-from")]
        public int PriceFrom
        {
            get => PriceRange.PriceFrom ?? 0;
            set
            {
                PriceRange.SetPriceFrom(value);
            }
        }
        [CommandOption("--price-to")]
        public int PriceTo
        {
            get => PriceRange.PriceTo ?? 0;
            set
            {
                PriceRange.SetPriceTo(value);

            }
        }

        public LocationCollection Locations { get; set; } = new LocationCollection();

        [CommandOption("--location")]
        [DefaultValue(new Location[]{})]
        public Location[] SelectedLocations
        {
            get => Locations.ToArray();
            set
            {
                try
                {
                    if (value is not null && value.Length > 0)
                    {
                        Locations.FromArray(value);
                        SelectedLocations = value;
                    }

                }
                catch (KeywordException ex)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
                }
            }
        }


        public StateCollection States { get; set; } = new StateCollection();
        [DefaultValue(new State[]{})]

        [CommandOption("--states")]
        public State[] SelectedStates
        {
            get => States.ToArray();
            set
            {
                try
                {
                    if (value is not null && value.Length > 0)
                    {
                        States.FromArray(value);
                        SelectedStates = value;
                    }

                }
                catch (KeywordException ex)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
                }
            }
        }
        
        public DayOfWeekCollection Days { get; set; } = new DayOfWeekCollection();

        [CommandOption("--days")]
        [DefaultValue(new DayOfWeek[]{})]
        public DayOfWeek[] SelectedDays
        {
            get => Days.ToArray();
            set
            {
                try
                {
                    Console.WriteLine($"Value1:{value}");

                    if (value is not null && value.Length > 0)
                    {
                        Console.WriteLine($"Value:{value}");
                        Days.FromArray(value);
                        SelectedDays = value;
                    }

                }
                catch (KeywordException ex)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
                }
            }
        }
        
        [CommandOption("--seller")]
        public Seller? Seller { get; set; }
        
        [CommandOption("--handover")]
        public Handover? Handover { get; set; }
        
        
        public TimeRange Time { get; set; } = new TimeRange();
        [CommandOption("--time-from")]
        public TimeOnly? TimeFrom
        {
            get => Time.From != TimeOnly.MinValue ? Time.From : (TimeOnly?)null;
            set => Time.From = value ?? TimeOnly.MinValue; 
        }

        public TimeOnly? TimeTo
        {
            get => Time.To != TimeOnly.MaxValue ? Time.To : (TimeOnly?)null;
            set => Time.To = value ?? TimeOnly.MaxValue; 
        }

        

        public enum ScraperType
        {
            WILLHABEN,
            EBAY,
            CUSTOM
        }
        
    }

    class JsonSettings
    {
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
                else
                {
                    return $"no_keywords_".GenerateRandomNDigitString();
                }
            }
        }

        public  Settings.ScraperType Type { get; set; }
        
        [JsonConverter(typeof(KeywordListJsonConverter<FuzzyKeyword>))]
        public List<FuzzyKeyword> FuzzyKeywords { get; set; } = new List<FuzzyKeyword>();
        
        [JsonConverter(typeof(KeywordListJsonConverter<ExactKeyword>))]
        public List<ExactKeyword> ExactKeywords { get; set; } = new List<ExactKeyword>();
        
        [JsonConverter(typeof(KeywordListJsonConverter<OmitKeyword>))]
        public List<OmitKeyword> OmitKeywords { get; set; } = new List<OmitKeyword>();
        public bool AsPresentOnly { get; set; } = false;
        public bool PayliveryOnly { get; set; }
        public PriceRange PriceRange { get; set; } = new PriceRange();
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<State> States { get; set; } = new List<State>();
        public Seller Seller { get; set; }
        public Handover Handover { get; set; }

        [JsonConverter(typeof(SimplifyableEnumCollectionConverter<DayOfWeek, DayOfWeekCollection>))]
        public DayOfWeekCollection Days { get; set; } = new DayOfWeekCollection();

        public TimeRange TimeRange { get; set; } = new TimeRange();
    }


    public  override int Execute(CommandContext context, Settings settings)
    {
        JsonSettings jsonSettings = new JsonSettings();
        jsonSettings.Type = settings.Type ?? PromptForScraperType();
        if (settings._fuzzyKeywords is not null || settings._exactKeywords is not null || settings._omitKeywords is not null)
        {
            ExplainKeywords();
        }

        jsonSettings.FuzzyKeywords = settings._fuzzyKeywords ?? PromptForKeywords<FuzzyKeyword>("fuzzy");
        jsonSettings.ExactKeywords = settings._exactKeywords ?? PromptForKeywords<ExactKeyword>("exact");
        jsonSettings.OmitKeywords = settings._omitKeywords ?? PromptForKeywords<OmitKeyword>("omitted");

        jsonSettings.AsPresentOnly = settings.AsPresentOnly ?? PromptForOnlyPresent();
        AnsiConsole.MarkupLine("testtest");
        AnsiConsole.MarkupLine($"{jsonSettings.AsPresentOnly}");

        AnsiConsole.MarkupLine($"{jsonSettings.PriceRange.PriceTo ?? 12}");

        if (jsonSettings.AsPresentOnly == false)
        {
            var isQueryingPrice = true;
            while (isQueryingPrice)
            {
                AnsiConsole.MarkupLine($"whatever");

                var priceFrom = PromptForInteger("Enter the minimum price of an item:", price => settings.PriceRange.IsValidPriceFrom(price));
                jsonSettings.PriceRange.SetPriceFrom(priceFrom);

                var priceTo = PromptForInteger("Enter the maximum price of an item:", price => settings.PriceRange.IsValidPriceTo(price));
                jsonSettings.PriceRange.SetPriceTo(priceTo);
                AnsiConsole.MarkupLine("321");
                if (priceTo == 0 && priceFrom == 0)
                {
                    isQueryingPrice =
                        !PromptForConfirmation("Setting both prices to 0 leads to only free items. Are you sure?");
                }
                else
                {
                    isQueryingPrice = false;
                }
            }
        }
        else
        {
            jsonSettings.PriceRange.SetPriceFrom(0);
            jsonSettings.PriceRange.SetPriceTo(0);
        }
        AnsiConsole.MarkupLine($"{settings.SelectedLocations is null}");
        AnsiConsole.MarkupLine("333");
        
        if (settings.SelectedLocations.Length > 0)
        {
            AnsiConsole.MarkupLine("123");
            jsonSettings.Locations = settings.Locations.ToList();
        }
        else
        {
            AnsiConsole.MarkupLine("999");

            jsonSettings.Locations = PromptForLocations().ToList();
        }
        
        if (settings.SelectedStates.Length > 0)
        {
            jsonSettings.States = settings.States.ToList();
        }
        else
        {
            jsonSettings.States = PromptForStates().ToList();
        }
        
        jsonSettings.PayliveryOnly = settings.PayliveryOnly ?? PromptForPaylivery();
        jsonSettings.Seller = settings.Seller ?? PromptForSeller();
        jsonSettings.Handover = settings.Handover ?? PromptForHandover();
        
        if (settings.SelectedDays.Length > 0)
        {

            jsonSettings.Days.FromArray(settings.SelectedDays);
        }
        else
        {

            jsonSettings.Days = PromptForDays();
        }

        jsonSettings.TimeRange.From = settings.Time.From ?? PromptForTime("At what time should the scraper start:", TimeOnly.MinValue, settings.Time.IsValidTimeFrom);
        
        

        var options = new JsonSerializerOptions
        {
            WriteIndented = true // Enable pretty-printing
        };
        using FileStream createStream = File.Create(@$"./Scrapers/{jsonSettings.Filename}.json");
        JsonSerializer.Serialize(createStream,jsonSettings, options);

        
        return 0;
    }

    
    private Settings.ScraperType PromptForScraperType()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<Settings.ScraperType>()
                .Title("For which website do you want to add a new scraper?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more scrapers)[/]")
                .AddChoices(Settings.ScraperType.WILLHABEN, Settings.ScraperType.EBAY, Settings.ScraperType.CUSTOM));
    }
    

    
    private bool PromptForConfirmation(string prompt)
    {
        var choice =  AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(prompt)
                .PageSize(3)
                .AddChoices("yes", "no"));
        return choice == "yes";
    }
    private bool PromptForOnlyPresent()
    {
        var choice =  AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Do you want to find only items that are for free?")
                .PageSize(3)
                .AddChoices("no", "yes"));
        return choice == "yes";
    }
    private int PromptForInteger(string question, Func<int, bool> validation)
    {
        while (true)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(question)
                    .AllowEmpty()
            );

            if (string.IsNullOrEmpty(input))
            {
                if (validation(0))
                {
                    return 0;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid integer.[/]");
                    continue;
                }
            }

            if (int.TryParse(input, out int result) && validation(result))
            {
                return result;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid integer.[/]");
            }
        }
    }

    
    private void ExplainKeywords()
    {
        AnsiConsole.MarkupLine("[underline red]Now it's time to define keywords.[/]");
        AnsiConsole.MarkupLine("There are [bold]3[/] types of keywords: [bold]Fuzzy, Exact and Omit[/]");
        AnsiConsole.MarkupLine("\t1. [bold green]'Fuzzy'[/] Keywords match words that can be part of a larger word. E.g. [bold green]bett[/] matches [bold green]betten[/], [bold green]bettgestell[/] and several others");
        AnsiConsole.MarkupLine("\t2. [bold green]'Exact'[/] Keywords match only that specific word. E.g. [bold green]bett[/] matches only [bold green]bett[/]");
        AnsiConsole.MarkupLine("\t3. [bold green]'Omit'[/] Keywords remove all results that contain this specific keyword. E.g. [bold green]bett[/] removes all results which contain [bold green]bett[/]");
        AnsiConsole.MarkupLine("[dim]If you don't want to search for any specific keyword, just press [bold]Enter[/] to continue without adding keywords.[/]");
    }

    public static LocationCollection PromptForLocations()
    {

        var prompt = new MultiSelectionPrompt<Location>()
            .Title("Select Locations")
            .PageSize(50)
            .NotRequired() // Don't require a selection
            .InstructionsText("[grey](Press [yellow]<space>[/] to toggle a location, [green]<enter>[/] to accept)[/]")
            .AddChoiceGroup(Location.WIEN, new List<Location>
            {
                Location.WIEN_01,
                Location.WIEN_02,
                Location.WIEN_03,
                Location.WIEN_04,
                Location.WIEN_05,
                Location.WIEN_06,
                Location.WIEN_07,
                Location.WIEN_08,
                Location.WIEN_09,
                Location.WIEN_10,
                Location.WIEN_11,
                Location.WIEN_12,
                Location.WIEN_13,
                Location.WIEN_14,
                Location.WIEN_15,
                Location.WIEN_16,
                Location.WIEN_17,
                Location.WIEN_18,
                Location.WIEN_19,
                Location.WIEN_20,
                Location.WIEN_21,
                Location.WIEN_22,
                Location.WIEN_23,
            })
            .AddChoiceGroup(Location.BURGENLAND, new List<Location>
            {
                Location.EISENSTADT,
                Location.RUST,
                Location.EISENSTADT_UMGEBUNG,
                Location.GUESSING,
                Location.JENNERSDORF,
                Location.MATTERSBURG,
                Location.NEUSIEDL,
                Location.OBERPULLENDORF,
                Location.OBERWART
            })
            .AddChoiceGroup(Location.KAERNTEN, new List<Location>
            {
                Location.KLAGENFURT,
                Location.VILLACH,
                Location.FELDKIRCHEN,
                Location.KLAGENFURT_LAND,
                Location.SANKT_VEIT,
                Location.SPITTAL,
                Location.VILLACH_LAND,
                Location.VOELKERMARKT,
                Location.WOLFSBERG,
                Location.HERMAGOR
            })
            .AddChoiceGroup(Location.VORARLBERG, new List<Location>
            {
                Location.BLUDENZ,
                Location.BREGENZ,
                Location.DORNBIRN,
                Location.FELDKIRCH,
            })
            .AddChoiceGroup(Location.SALZBURG, new List<Location>()
            {
                Location.SALZBURG_STADT,
                Location.HALLEIN,
                Location.SALZBURG_UMGEBUNG,
                Location.ST_JOHANN,
                Location.TAMSWEG,
                Location.ZELL_AM_SEE
            })
            .AddChoiceGroup(Location.OBEROESTERREICH, new List<Location>()
            {
                Location.LINZ,
                Location.STEYR,
                Location.WELS,
                Location.BRAUNAU,
                Location.EFERDING,
                Location.FREISTADT,
                Location.GMUNDEN,
                Location.GRIESKIRCHEN,
                Location.KIRCHDORF_AN_DER_KREMS,
                Location.LINZ_LAND ,
                Location.PERG,
                Location.RIED,
                Location.ROHRBACH,
                Location.SCHAERDING,
                Location.STEYR_LAND,
                Location.URFAHR_UMGEBUNG,
                Location.VOECKLABRUCK,
                Location.WELS_LAND ,
            })
            .AddChoiceGroup(Location.STEIERMARK, new List<Location>()
            {
                Location.GRAZ,
                Location.DEUTSCHLANDSBERG,
                Location.GRAZ_UMGEBUNG,
                Location.LEIBNITZ,
                Location.LEOBEN,
                Location.LIETZEN,
                Location.MURAU,
                Location.VOITSBERG,
                Location.WEITZ,
                Location.MURTAL,
                Location.BRUCK_MUERZZUSCHLAG,
                Location.HARTBERG_FUERSTENFELD,
                Location.SUEDOSTSTEIERMARK,
            })
            .AddChoiceGroup(Location.NIEDEROESTERREICH, new List<Location>()
            {
                Location.KREMS_AN_DER_DONAU ,
                Location.SANKT_POELTEN ,
                Location.WAIDHOFEN_AN_DER_YBBS,
                Location.WIENER_NEUSTADT ,
                Location.AMSTETTEN ,
                Location.BADEN ,
                Location.BRUCK_AN_DER_LEITHA,
                Location.GAENSERDORF,
                Location.GMUEND,
                Location.HOLLABRUNN,
                Location.HORN,
                Location.KORNEUBURG,
                Location.KREMS_LAND,
                Location.LILIENFELD,
                Location.MELK,
                Location.MISTELBACH,
                Location.MOEDLING,
                Location.NEUNKIRCHEN,
                Location.SANKT_POELTEN_LAND,
                Location.SCHEIBBS,
                Location.TULLN,
                Location.WAIDHOFEN_AN_DER_THAYA,
                Location.WIENER_NEUSTADT_LAND,
                Location.ZWETTL,
            })
            .AddChoiceGroup(Location.TIROL, new List<Location>
            {
                Location.INNSBRUCK,
                Location.IMST,
                Location.INNSBRUCK_LAND,
                Location.KITZBUEHEL,
                Location.KUFSTEIN,
                Location.LANDECK,
                Location.LIENZ,
                Location.REUTTE,
                Location.SCHWAZ,
            });

        var collection = new LocationCollection();
        collection.FromList(AnsiConsole.Prompt(prompt));
        return collection;
    }

    private Seller PromptForSeller()
    {
        var choice =  AnsiConsole.Prompt(
            new SelectionPrompt<Seller>()
                .Title("Which type of Seller do you prefer?")
                .PageSize(3)
                .AddChoices(new []{Seller.BOTH,Seller.PRIVATE, Seller.COMMERCIAL, }));
        return choice;
    }
    private Handover PromptForHandover()
    {
        var choice =  AnsiConsole.Prompt(
            new SelectionPrompt<Handover>()
                .Title("Which type of Handover do you prefer?")
                .PageSize(3)
                .AddChoices(new []{Handover.BOTH, Handover.SHIPMENT, Handover.SELFCOLLECTION}));
        return choice;
    }

    private TimeOnly PromptForTime(string prompt, TimeOnly defaultValue, Func<TimeOnly, bool> validation)
    {
        while (true)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                    .AllowEmpty()
                    .PromptStyle("green")
            );

            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            if (TimeOnly.TryParse(input, out TimeOnly result))
            {
                if (validation(result))
                {
                    return result;
                }
            }

            AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid time in the format: HH:mm.[/]");
        }
    }

    private bool PromptForPaylivery()
    {
        var choice =  AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Do you want to find only items that offer paylivery?")
                .PageSize(3)
                .AddChoices("no", "yes"));
        return choice == "yes";
    }
    private StateCollection PromptForStates()
    {
        var prompt = new MultiSelectionPrompt<State>()
            .Title("Select Locations")
            .PageSize(50)
            .NotRequired()
            .InstructionsText("[grey](Press [yellow]<space>[/] to toggle a state, [green]<enter>[/] to accept. If no state is selected, the scraper will look for any state)[/]")
            .AddChoices(new List<State>
            {
                State.NEU,
                State.DEFEKT,
                State.GEBRAUCHT,
                State.NEUWERTIG,
                State.GENERALÜBERHOLT,
                State.AUSSTELLUNGSSTÜCK,
            });
        var collection = new StateCollection();
        collection.FromList(AnsiConsole.Prompt(prompt));
        return collection;
    }
    private DayOfWeekCollection PromptForDays()
    {
        var prompt = new MultiSelectionPrompt<DayOfWeek>()
            .Title("Select Days")
            .PageSize(50)
            .NotRequired()
            .InstructionsText("[grey](Press [yellow]<space>[/] to toggle a state, [green]<enter>[/] to accept. If no state is selected, the scraper will look for any state)[/]")
            .AddChoices(new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday,

            });
        var collection = new DayOfWeekCollection();
        collection.FromList(AnsiConsole.Prompt(prompt));
        return collection;
    }
    private List<TKeyword> PromptForKeywords<TKeyword>(string keywordType) where TKeyword : Keyword
    {
        while (true)
        {
            AnsiConsole.MarkupLine($"\nPlease enter [bold green]{keywordType}[/] keywords separated by commas:");

            var input = AnsiConsole.Prompt(
                new TextPrompt<string>("Keywords:").AllowEmpty()
            );

            if (string.IsNullOrEmpty(input))
            {
                // If the input is empty, return an empty list
                return new List<TKeyword>();
            }

            try
            {
                // Create a list to store the keywords
                var keywordList = new List<TKeyword>();

                // Create a factory method based on the type of TKeyword
                TKeyword CreateKeyword(string term) => (TKeyword)Activator.CreateInstance(typeof(TKeyword), term);

                // Process each keyword from the input
                foreach (var keywordString in input.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    var keyword = CreateKeyword(keywordString.Trim());
                    Keyword.AddUnique(keywordList, keyword); // Assuming AddUnique method is available
                }

                return keywordList;
            }
            catch (KeywordException ex)
            {
                // If there's an error, display a message and repeat the loop
                AnsiConsole.MarkupLine("[red]Error:[/] {0}", ex.Message);
            }
        }
    }
}
