using System.ComponentModel;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Willhaben.Domain.Settings;

public class GlobalSettings
{
    [JsonIgnore] private  int _minBreakBetweenScrapes { get; set; } = 30;

    public  int MinBreakBetweenScrapes
    {
        get => _minBreakBetweenScrapes;
        set
        {
            _minBreakBetweenScrapes = value;
        }
    }

    public LoggingSettings Logging { get; set; } = new();
    public ConnectionSettings Connection { get; set; } = new();
    public List<string> UserAgents { get; set; } = new();
    
    public void ToJson(string relativePath = @"Settings/Global/globalSettings.json")
    {
        var directoryPath = Path.GetDirectoryName(relativePath);
        
        // Check if directory exists
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true // Enable pretty-printing
        };

        using FileStream createStream = File.Create(relativePath);
        JsonSerializer.Serialize(createStream, this, options);
    }

    public GlobalSettings()
    {
        MinBreakBetweenScrapes = 30;
        Logging = new LoggingSettings();
        Connection = new ConnectionSettings();
        UserAgents = new List<string>();
    }
    public GlobalSettings(string filePath = @"Settings/Global/globalSettings.json")
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
            
            /*
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Allow
            };
            */
            
            var settings = JsonSerializer.Deserialize<GlobalSettings>(json);
            if (settings is not null)
            {
                MinBreakBetweenScrapes = settings.MinBreakBetweenScrapes;
                Logging = settings.Logging;
                Connection = settings.Connection;
                UserAgents = settings.UserAgents;
            }
        }
        else
        {
            throw new FileNotFoundException($"Settings file '{filePath}' not found.");
        }
    }
    
}