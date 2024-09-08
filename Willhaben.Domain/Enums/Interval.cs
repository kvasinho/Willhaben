namespace Willhaben.Domain.Models;


public enum Interval
{
    M1 = 60,

    M2 = 120,

    M5 = 300,

    M10 = 600,

    M15 = 900,

    M30 = 1800,

    H1 = 3600,

    H2 = 7200,

    H4 = 14400,

    H6 = 21600,

    H12 = 43200,
        
    D1 = 86400
        
}

public static class IntervalExtensions
{
    public static TimeSpan ToTimeSpan(this Interval interval)
    {
        return TimeSpan.FromMinutes((int)interval);
    }
}
