using Willhaben.Domain.Models;

namespace Willhaben.Domain.Settings;

public  class LoggingSettings
{
    public LogLevel LogLevel { get; set; }
    public LogRotation LogRotation { get; set; }
    public LoggingSettings(LogLevel logLevel = LogLevel.INFO, LogRotation logRotation = LogRotation.DAILY)
    {
        LogLevel = logLevel;
        LogRotation = logRotation;
    }
}