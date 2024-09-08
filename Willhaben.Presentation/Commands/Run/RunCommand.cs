using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Manager.QueueManagerService;
using Willhaben.Scraper.Implementations;

namespace Willhaben.Presentation.Commands.Run
{
    public class RunCommand : AsyncCommand<RunSettings>
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IScraperQueueServiceFactory _scraperQueueServiceFactory;
        private readonly List<ScraperQueueService> _scraperQueueServices;

        public RunCommand(
            IScraperQueueServiceFactory scraperQueueServiceFactory,
            GlobalSettings globalSettings)
        {
            _scraperQueueServiceFactory = scraperQueueServiceFactory;
            _globalSettings = globalSettings;
            _scraperQueueServices = new List<ScraperQueueService>();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, RunSettings settings)
        {
            AnsiConsole.MarkupLine($"\n\n");

            var scrapers = await JsonScraperFactory.LoadScrapersFromDirectoryAsync(@"Settings/Scrapers");


            if (scrapers.Any())
            {

                await scrapers[0].ScrapeAsync();
            }

            return 0;

            AnsiConsole.MarkupLine($"Loaded [bold green]{scrapers.Count}[/] scrapers.");
            
            var groupedScrapers = scrapers.GroupBy(scraper => scraper.Key);
            
            foreach (var group in groupedScrapers)
            {
                var key = group.Key;
                AnsiConsole.MarkupLine($"\tKey: [bold green]{key.Value}[/], Scrapers: [bold green]{group.Count()}[/]");
                var scraperQueueService = _scraperQueueServiceFactory.Create();

                scraperQueueService.AddScrapers(group.ToList());

                _scraperQueueServices.Add(scraperQueueService);
            }
            
            
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                AnsiConsole.MarkupLine("[red]Shutdown initiated, please wait...[/]");
                cts.Cancel();
                args.Cancel = true; // Prevent immediate process termination
            };
            try
            {
                foreach (var serivce in _scraperQueueServices)
                {
                    await serivce.RunAsync(cts.Token);
                }

            }
            catch (OperationCanceledException)
            {
                AnsiConsole.MarkupLine("[yellow]Operation was cancelled.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            }
    
            return 0;
        }
    }
}