using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Willhaben.Scraper.Implementations;

namespace Willhaben.Manager.QueueManagerService
{
    public class ScraperQueueService
    {
        private readonly GlobalSettings _globalSettings;
        private Key Key { get; set; }
        private readonly HashSet<Guid> _scraperIds = new();
        private readonly PriorityQueue<IScraper, DateTime> _queue = new();
        private DateTime _nextRunTime = DateTime.MinValue;

        private DateTime NextPossibleGlobalSettingsRestrictedRunTime =>
            DateTime.UtcNow.AddSeconds(_globalSettings.MinBreakBetweenScrapes);

        public int Count => _queue.Count;

        public ScraperQueueService(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public void AddScraper(IScraper scraper)
        {
            if (Key is null)
            {
                Key = scraper.Key;
            }

            if (!Key.Equals(scraper.Key))
            {
                throw new ArgumentException("Scraper key must match the queue key");
            }

            if (!_scraperIds.Add(scraper.ScraperSettings.Id))
            {
                throw new ArgumentException($"Scraper with Id: {scraper.ScraperSettings.Id} already exists in the queue");
            }
            EnqueueScraper(scraper, true);
        }

        public void AddScrapers(List<IScraper> scrapers)
        {
            scrapers.ForEach(AddScraper);
        }

        
        private void EnqueueScraper(IScraper scraper, bool initialEnqueue)
        {
            if (initialEnqueue)
            {
                _queue.Enqueue(scraper, scraper.ScraperSettings.ScrapingScheduleSettings.NextPossibleRuntime);
                //Console.WriteLine($"INITIAL: Enqueued Scraper: {scraper.ScraperSettings.Id} AT {DateTime.UtcNow} FOR {scraper.ScraperSettings.ScrapingScheduleSettings.NextPossibleRuntime}");
            }
            else
            {
                _queue.Enqueue(scraper, scraper.ScraperSettings.ScrapingScheduleSettings.GetNextPossibleRunTimeAfterInterval);
                //Console.WriteLine($"CONSEQUTIVE: Enqueued Scraper: {scraper.ScraperSettings.Id} AT {DateTime.UtcNow} FOR {scraper.ScraperSettings.ScrapingScheduleSettings.GetNextPossibleRunTimeAfterInterval}");
            }
            UpdateNextRunTime();
        }
        

        private void UpdateNextRunTime()
        {
            var nextQueueRunTime = _queue.TryPeek(out _, out DateTime nextRunTime) ? nextRunTime : DateTime.MaxValue;
            _nextRunTime = nextQueueRunTime > NextPossibleGlobalSettingsRestrictedRunTime
                ? nextQueueRunTime
                : NextPossibleGlobalSettingsRestrictedRunTime;

            //Console.WriteLine($"UPDATED NEXT: {_nextRunTime} (SCRAPER: {nextQueueRunTime}, RESTRICTED: {NextPossibleGlobalSettingsRestrictedRunTime})");
                
        }

        private async Task RunScraper(IScraper scraper, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            try
            {
                object rawResult = await scraper.ScrapeAsync();

                if (rawResult is IScrapingResult scrapingResult)
                {

                    Type resultType = rawResult.GetType();
                    if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ScrapingResult<>))
                    {
                        // Get the Data property using reflection
                        PropertyInfo dataProperty = resultType.GetProperty("Data");
                        if (dataProperty != null)
                        {
                            object dataValue = dataProperty.GetValue(rawResult);
                            if (dataValue is ICollection collection)
                            {
                                //Console.WriteLine($"This is a ScrapingResult with {collection.Count} items");
                                //Console.WriteLine($"The type of items in the collection is: {resultType.GetGenericArguments()[0].Name}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Scrape result for {scraper.ScraperSettings.Id} is not of a recognized ScrapingResult type");
                }
                

                // Re-enqueue the scraper with its NextPossibleRun (recalculated on the Fly)
                EnqueueScraper(scraper, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while running scraper {scraper.ScraperSettings.Id}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Optionally handle the failure logic if needed, for now just re-enqueue
                EnqueueScraper(scraper, false);
            }

            // Update the next run time for scheduling
            UpdateNextRunTime();
        }

        private async Task ProcessQueue(CancellationToken cancellationToken)
        {
            try
            {
                if (_queue.TryDequeue(out var scraper, out var _))
                {
                    await RunScraper(scraper, cancellationToken);
                }
                else
                {
                    _nextRunTime = DateTime.MaxValue; // No scrapers left to run
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ProcessQueue: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Calculate the delay until the next run time
                TimeSpan delay = _nextRunTime - DateTime.UtcNow;
                //Console.WriteLine($"NextrunTime: {_nextRunTime}");
                //Console.WriteLine($"Delay: {delay}");

                // If the delay is negative or zero, we should process the queue immediately
                if (delay <= TimeSpan.Zero)
                {
                    // Process the queue and update the next run time
                    await ProcessQueue(cancellationToken);

                }
                else
                {
                    // Wait for the delay or until cancellation is requested
                    try
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        // Task was cancelled, handle if necessary
                        // Optionally log or handle the cancellation
                    }
                }
            }
        }

    }
}
