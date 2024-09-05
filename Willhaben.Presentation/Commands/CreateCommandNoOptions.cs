using System.ComponentModel;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Domain.Utils;
using Willhaben.Domain.Utils.Converters;
using Willhaben.Presentation.Commands;

namespace Willhaben.Presentation.Commands;


public class CreateCommandNoOptions : Command<CreateCommandNoOptions.Settings>
{
    public class Settings : CommandSettings
    {
    }
    
    public IJsonSettings CreateSettings(ScraperType scraperType)
    {
        return scraperType switch
        {
            ScraperType.WILLHABEN => new WillhabenJsonSettings(),
            _ => throw new ArgumentOutOfRangeException(nameof(scraperType),
                $"No settings available for scraper type {scraperType}.")
        };
    }


    public  override int Execute(CommandContext context, Settings settings)
    {
        var type = PromptForType();
        var jsonSettings = CreateSettings(type);

        if (jsonSettings is WillhabenJsonSettings willhaben)
        {
            ExplainKeywords();

            willhaben.FuzzyKeywords = PromptForKeywords<FuzzyKeyword>("fuzzy");
            willhaben.ExactKeywords = PromptForKeywords<ExactKeyword>("exact");
            willhaben.OmitKeywords = PromptForKeywords<OmitKeyword>("omitted");
            
            willhaben.AsPresentOnly =  PromptForOnlyPresent();
            
            if (willhaben.AsPresentOnly == false)
            {
                var isQueryingPrice = true;
                while (isQueryingPrice)
                {

                    var priceFrom = PromptForInteger("Enter the minimum price of an item:", price => willhaben.PriceRange.IsValidPriceFrom(price));
                    willhaben.PriceRange.SetPriceFrom(priceFrom);

                    var priceTo = PromptForInteger("Enter the maximum price of an item:", price => willhaben.PriceRange.IsValidPriceTo(price));
                    willhaben.PriceRange.SetPriceTo(priceTo);
                    if (priceTo == 0 && priceFrom == 0)
                    {
                        isQueryingPrice =
                            !PromptForConfirmation("Setting both prices to 0 leads to only free items. Are you sure?");
                    }
                    else
                    {
                        willhaben.AsPresentOnly = true;
                        isQueryingPrice = false;
                    }
                }
            }
            else
            {
                willhaben.PriceRange.SetPriceFrom(0);
                willhaben.PriceRange.SetPriceTo(0);
            }


            willhaben.Locations = PromptForLocations();
            willhaben.States = PromptForStates();
            willhaben.PayliveryOnly = PromptForPaylivery();
            willhaben.Seller = PromptForSeller();
            willhaben.Handover = PromptForHandover();
            willhaben.Rows = PromptForInteger("How many items do you want to scrape? Default: 100",
                i => i > 0 && i <= 200, 100);
            willhaben.ScraperSettings.Days = PromptForDays().ToList();
            willhaben.ScraperSettings.From = PromptForTime("At what time should the scraper start:", TimeOnly.MinValue, willhaben.ScraperSettings.IsValidTimeFrom);
            willhaben.ScraperSettings.To = PromptForTime("At what time should the scraper end:", TimeOnly.MaxValue, willhaben.ScraperSettings.IsValidTimeTo);
            willhaben.ScraperSettings.Interval = PromptForInterval();
            
            AnsiConsole.MarkupLine(willhaben.Url);
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Enable pretty-printing
            };
            using FileStream createStream = File.Create(@$"./Scrapers/{willhaben.Filename}.json");
            JsonSerializer.Serialize(createStream,willhaben, options);
        }
        
        
        return 0;
    }

    
    private ScraperType PromptForScraperType()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<ScraperType>()
                .Title("For which website do you want to add a new scraper?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more scrapers)[/]")
                .AddChoices(ScraperType.WILLHABEN, ScraperType.EBAY, ScraperType.CUSTOM));
    }
    
    private Interval PromptForInterval()
    {
        Dictionary<string, Interval> intervals = new Dictionary<string, Interval>()
        {
            { "Minute", Interval.M1 },
            { "5 Minutes", Interval.M5 },
            { "2 Minutes", Interval.M2 },
            { "10 Minutes", Interval.M10 },
            { "15 Minutes", Interval.M15 },
            { "30 Minutes", Interval.M30 },
            { "Hour", Interval.H1 },
            { "2 Hours", Interval.H2 },
            { "4 Hours", Interval.H4 },
            { "6 Hours", Interval.H6 },
            { "12 Hours", Interval.H12 },
            { "Day", Interval.D1}
        };
        var interval  =AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("How often do you want to let the scraper run? Every..")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more scrapers)[/]")
                .AddChoices(intervals.Keys));
        
        return intervals[interval];
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
    private int PromptForInteger(string question, Func<int, bool> validation, int defaultValue = 0)
    {
        while (true)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(question)
                    .AllowEmpty()
            );

            if (string.IsNullOrEmpty(input))
            {
                if (validation(defaultValue))
                {
                    return defaultValue;
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
    
    private ScraperType PromptForType()
    {
        var choice =  AnsiConsole.Prompt(
            new SelectionPrompt<ScraperType>()
                .Title("Which type of scraper do you want to implement")
                .PageSize(3)
                .AddChoices(new []{ScraperType.WILLHABEN,ScraperType.EBAY, ScraperType.CUSTOM}));
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
