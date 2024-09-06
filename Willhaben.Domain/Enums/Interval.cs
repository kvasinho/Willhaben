namespace Willhaben.Domain.Models;


public enum Interval
{
    M1 = 1,

    M2 = 2,

    M5 = 5,

    M10 = 10,

    M15 = 15,

    M30 = 30,

    H1 = 60,

    H2 = 120,

    H4 = 240,

    H6 = 360,

    H12 = 720,
        
    D1 = 1440
        
}

public static class IntervalExtensions
{
    public static TimeSpan ToTimeSpan(this Interval interval)
    {
        return TimeSpan.FromMinutes((int)interval);
    }
}
