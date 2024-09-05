using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Willhaben.Domain.Models;

public  class Logging
{
    public LogLevel LogLevel { get; set; }
    public LogRotation LogRotation { get; set; }
    public Logging(LogLevel logLevel = LogLevel.INFO, LogRotation logRotation = LogRotation.DAILY)
    {
        LogLevel = logLevel;
        LogRotation = logRotation;
    }
}
public class Connection
    {
        public Connection(int requestTimeout = 10, int connectionTimeout = 10, int maxRetries = 3)
        {
            RequestTimeout = requestTimeout;
            ConnectionTimeout = connectionTimeout;
            MaxRetries = maxRetries;
        }
        [JsonIgnore] private int _requestTimeout { get; set; }

        public int RequestTimeout
        {
            get => _requestTimeout;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Timeout cannot be less than 1");
                }

                _requestTimeout = value;
            }
        }

        [JsonIgnore] private int _connectionTimeout { get; set; }

        public int ConnectionTimeout
        {
            get => _connectionTimeout;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Timeout cannot be less than 1");
                }

                _connectionTimeout = value;
            }
        }

        [JsonIgnore] private int _maxRetries { get; set; }

        public int MaxRetries
        {
            get => _maxRetries;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Retries cannot be less than 0");
                }

                _maxRetries = value;
            }
        }
    }
public class GlobalSettings
{
    [JsonIgnore] private  int _minBreakBetweenScrapes { get; set; } = 30;

    public  int MinBreakBetweenScrapes
    {
        get => _minBreakBetweenScrapes;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Time cannot be less than 1");
            }

            if (value < 30)
            {
                throw new WarningException(
                    "Scraping too frequently may lead to getting banned. Recommended freuqncy is 30 seconds an up");
            }

            if (value > 60)
            {
                throw new WarningException(
                    "Scraping too infrequently may lead to jobs not being able to finish in the requested interval. Recommended frequency is 60 seconds or less");
            }

            _minBreakBetweenScrapes = value;
        }
    }

    public Logging Logging { get; set; } = new();
    public Connection Connection { get; set; } = new();
    
    public void ToJson(string path = @"../../Settings/Global/globalSettings.json")
    {
        var directoryPath = Path.GetDirectoryName(path);
        
        // Check if directory exists
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true // Enable pretty-printing
        };

        using FileStream createStream = File.Create(path);
        JsonSerializer.Serialize(createStream, this, options);
    }

    public GlobalSettings()
    {
        MinBreakBetweenScrapes = 30;
        Logging = new Logging();
        Connection = new Connection();
    }
    public GlobalSettings(string filePath = @"../../Settings/Global/globalSettings.json")
    {
        // Get directory part of the path
        var directoryPath = Path.GetDirectoryName(filePath);


        // Check if directory exists before attempting to load the file
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
        }

        // Check if the file itself exists
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<GlobalSettings>(json);
            if (settings is not null)
            {
                MinBreakBetweenScrapes = settings.MinBreakBetweenScrapes;
                Logging = settings.Logging;
                Connection = settings.Connection;
            }
        }
        else
        {
            throw new FileNotFoundException($"Settings file '{filePath}' not found.");
        }
    }
    
}