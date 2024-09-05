
using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Models;
using Willhaben.Presentation.Commands;


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
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<CreateCommandNoOptions>("create");

                config.AddBranch<SettingsSettings>("settings", add =>
                {
                    add.AddCommand<EditGlobalSettingsCommand>("edit");
                    //Add View COmmand
                });    

            });

            return app.Run(args);

            
        }
    }
}