namespace Willhaben.Domain.Models;

public interface IScraperSettings
{
    public ICollection<DayOfWeek> Days { get; set; }
    public TimeOnly From { get; set; }
    public TimeOnly To { get; set; }
    public Interval Interval { get; set; }
    public bool IsValidTimeFrom(TimeOnly from);
    public bool IsValidTimeTo(TimeOnly to);
}

public class ScraperSettings : IScraperSettings
{
    public ICollection<DayOfWeek> Days { get; set; } = new List<DayOfWeek>();
    public TimeOnly From { get; set; } = TimeOnly.MinValue;
    public TimeOnly To { get; set; } = TimeOnly.MaxValue;
    public Interval Interval { get; set; } = Interval.M5;

    public bool IsValidTimeFrom(TimeOnly from)
    {
        return from != To;
    }

    public bool IsValidTimeTo(TimeOnly to)
    {
        return to != From;
    }
}