using System.Text.Json.Serialization;

namespace Willhaben.Domain.Settings;

public class ConnectionSettings
{
    public ConnectionSettings(int requestTimeout = 10, int connectionTimeout = 10, int maxRetries = 3)
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