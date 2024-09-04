
using Spectre.Console;
using Spectre.Console.Cli;
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
                config.AddCommand<CreateCommand>("create");
            });

            return app.Run(args);

            
        }
    }
}