using Willhaben.Domain.Settings;

namespace Willhaben.Manager.QueueManagerService;

public interface IScraperQueueServiceFactory
{
    ScraperQueueService Create();
}
public class ScraperQueueServiceFactory: IScraperQueueServiceFactory
{
    private readonly GlobalSettings _globalSettings;

    public ScraperQueueServiceFactory(GlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }
    public ScraperQueueService Create()
    {
        return new ScraperQueueService(_globalSettings);
    }
}