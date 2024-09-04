using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using Willhaben.Domain.Models;
using Willhaben.Scraper.Products;

namespace MyConsoleApp
{
    class Program
    {

        static async Task Main(string[] args)
        {
            // Build and configure the host
            /*
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register services here
                    services.AddSingleton<IUrlBuilderService, UrlBuilderService>();
                })
                .Build();
            */
            // Resolve and run the service
            string path = "/Users/michaelkvasin/csharp/Willhaben/Willhaben.Scraper/DummyScraper.json";
            var jsonConfigFactory = new JsonConfigFactory();
            var builder = new WillhabenUrlBuilder();
            JsonWillhabenUrlService converter = new JsonWillhabenUrlService(builder,jsonConfigFactory, path);
            await converter.ParseAsync();


        }
    }
}