using MediatR;
using Willhaben.Domain.Settings;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Scraper;

namespace Willhaben.Manager;


/*
public class ScraperResultNotification<T> : INotification
{
    public Scraper Scraper { get; }
    public ScrapingResult<T> Result { get; }

    public ScraperResultNotification(Scraper scraper, ScrapingResult<T> result)
    {
        Scraper = scraper;
        Result = result;
    }
}
public class ScraperQueueService
{
    private readonly GlobalSettings _globalSettings;
    private readonly Key _key;
    private readonly HashSet<string> _scraperIds = new HashSet<string>();
    private readonly PriorityQueue<Scraper, DateTime> _queue = new PriorityQueue<Scraper, DateTime>();
    private DateTime _nextRunTime = DateTime.MinValue;

    public ScraperQueueService(GlobalSettings globalSettings, Key key)
    {
        _globalSettings = globalSettings;
        _key = key;
    }

    public void AddScraper(Scraper scraper)
    {
        if (!_key.Equals(scraper.Key))
        {
            throw new ArgumentException("Scraper key must match the queue key");
        }

        if (!_scraperIds.Add(scraper.Id))
        {
            throw new ArgumentException($"Scraper with Id: {scraper.Id} already exists in the queue");
        }

        EnqueueScraper(scraper);
    }

    public void AddScrapers(List<Scraper> scrapers)
    {
        scrapers.ForEach(AddScraper);
    }

    private void EnqueueScraper(Scraper scraper)
    {
        _queue.Enqueue(scraper, scraper.ScraperSettings.NextPossibleRun);
        UpdateNextRunTime();
    }

    private void UpdateNextRunTime()
    {
        _nextRunTime = _queue.TryPeek(out _, out var nextRunTime) ? nextRunTime : DateTime.MaxValue;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var delay = _nextRunTime - DateTime.UtcNow;

            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, cancellationToken);
            }

            if (_nextRunTime <= DateTime.UtcNow)
            {
                await ProcessQueue(cancellationToken);
            }
        }
    }

    private async Task ProcessQueue(CancellationToken cancellationToken)
    {
        if (_queue.TryDequeue(out var scraper, out var _))
        {
            await RunScraper(scraper, cancellationToken);
        }
        else
        {
            _nextRunTime = DateTime.MaxValue;
        }
    }

    private async Task RunScraper(Scraper scraper, CancellationToken cancellationToken)
    {
        try
        {
            var result = await scraper.Scrape<WillhabenResult>();
            Console.WriteLine($"Scrape result for {scraper.Id}: {result.Success} at {DateTime.Now}");

            // Re-enqueue the scraper regardless of the result
            EnqueueScraper(scraper);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while running scraper {scraper.Id}: {ex.Message}");
            // Re-enqueue the scraper even if an exception occurred
            EnqueueScraper(scraper);
        }

        // Update the next run time for the queue
        _nextRunTime = DateTime.UtcNow.AddSeconds(_globalSettings.MinBreakBetweenScrapes);
        UpdateNextRunTime();
    }
}

/*
private async Task PublishResult<T>(Scraper scraper, ScrapingResult<T> result)
{
    var notification = new ScraperResultNotification<T>(scraper, result);
    await _mediator.Publish(notification);
}

private async Task HandleRetry(Key key, Scraper scraper, QueueState queueState, StatusCode statusCode)
{
    if (scraper.ScraperSettings.RetryCount < _globalSettings.Connection.MaxRetries)
    {
        scraper.ScraperSettings.RetryCount++;
        var delay = CalculateRetryDelay(scraper.ScraperSettings.RetryCount, statusCode);
        scraper.ScraperSettings.NextPossibleRun = DateTime.UtcNow.Add(delay);
        queueState.RetryQueue.Enqueue(scraper);
        _logger.LogWarning("Scraper {ScraperId} scheduled for retry {RetryCount} in {Delay}", scraper.Id, scraper.ScraperSettings.RetryCount, delay);
    }
    else
    {
        _logger.LogError("Scraper {ScraperId} exceeded max retry attempts", scraper.Id);
        ReEnqueueScraper(key, scraper, queueState, calculateNextRun: true);
        // Additional logic for handling max retries exceeded (e.g., notify admin)
    }
}


private TimeSpan CalculateRetryDelay(int retryCount, StatusCode statusCode)
{
    // Implement your retry delay strategy here
    // For example, exponential backoff with jitter
    var baseDelay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
    var jitter = TimeSpan.FromMilliseconds(new Random().Next(0, 1000));
    return baseDelay + jitter;
}
*/


/*
public class ScraperResultLogger<T> : INotificationHandler<ScraperResultNotification<T>>
{
    private readonly ILogger<ScraperResultLogger<T>> _logger;

    public ScraperResultLogger(ILogger<ScraperResultLogger<T>> logger)
    {
        _logger = logger;
    }

    public Task Handle(ScraperResultNotification<T> notification, CancellationToken cancellationToken)
    {
        if (notification.Result.Success)
        {
            _logger.LogInformation("Scraper {ScraperId} completed successfully", notification.Scraper.Id);
        }
        else
        {
            _logger.LogWarning("Scraper {ScraperId} failed: {ErrorMessage}", notification.Scraper.Id, notification.Result.ErrorMessage);
        }

        return Task.CompletedTask;
    }
}

*/