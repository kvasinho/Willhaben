using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
//using Willhaben.Manager;

namespace Willhaben.Presentation.Commands.Run;

public class RunCommand : AsyncCommand<RunSettings>
{
    

    
    //private readonly ScraperQueueService _scraperQueueService;
    private readonly GlobalSettings _globalSettings;

    public RunCommand(
        //ScraperQueueService scraperQueueService,
        GlobalSettings globalSettings)
    {
        //_scraperQueueService = scraperQueueService;
        _globalSettings = globalSettings;
    }


    public  override async  Task<int> ExecuteAsync(CommandContext context, RunSettings settings)
    {
        var res = await JsonScraperFactory.LoadScrapersFromDirectoryAsync();
        
        //Console Logs of Loaded Scrapers
        AnsiConsole.MarkupLine($"Loaded [bold green]{res.Count}[/] scrapers.");
        foreach (var r in res.GroupBy(r => r.Key))
        {
            AnsiConsole.MarkupLine($"\tKey: [bold green]{r.Key.Value}[/], Scrapers: [bold green]{r.Count()}[/]");
        }
        
        
        return 0;
    }
        
        
}
