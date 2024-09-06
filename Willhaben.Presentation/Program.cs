
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Infrastructure.DependencyInjection;
//using Willhaben.Manager;
using Willhaben.Presentation.Commands;
using Willhaben.Presentation.Commands.Run;


namespace Willhaben.Presentation
{
    class Program
    {

        public  static int  Main(string[] args)
        {
            //Commands
            //- Create: Creates a new scraper 
            // Edit: Edits an existing scraper 
            //Delete: Removes a scraper 
            //Run: Runs all scrapers 
            
            var services = new ServiceCollection();
            //services.AddScoped<ScraperQueueService>();
            services.AddSingleton<GlobalSettings>();


            var registrar = new TypeRegistrar(services);

            var app = new CommandApp<RunCommand>(registrar);

            app.Configure(config =>
            {
                config.AddCommand<CreateCommandNoOptions>("create");
                config.AddCommand<RunCommand>("run");
                

                config.AddBranch<SettingsSettings>("settings", add =>
                {
                    add.AddCommand<EditGlobalSettingsCommand>("edit");
                });    

            });

            return app.Run(args);

            
        }
    }
}