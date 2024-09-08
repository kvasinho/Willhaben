using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Models;
using Willhaben.Domain.Settings;
using Willhaben.Infrastructure.DependencyInjection;
using Willhaben.Manager.QueueManagerService;
using Willhaben.Presentation.Commands;
using Willhaben.Presentation.Commands.Run;

namespace Willhaben.Presentation
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ScraperQueueService>();
            services.AddSingleton<GlobalSettings>();
            services.AddSingleton<IScraperQueueServiceFactory,ScraperQueueServiceFactory>();
            services.AddScoped<HttpClient>();
            //services.AddMediatR();
            
            var registrar = new TypeRegistrar(services);

            var app = new CommandApp(registrar);

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