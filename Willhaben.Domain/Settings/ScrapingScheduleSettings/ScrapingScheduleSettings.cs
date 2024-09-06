using System.Text.Json.Serialization;

namespace Willhaben.Domain.Models;


/// <summary>
/// This Settings class Is necessary in all scrapers.
/// It contains the base settings when to scrape
/// </summary>
public class ScrapingScheduleSettings
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
    
    public (DateTime Start, DateTime End)? ActiveInterval
    {
        get
        {
            DateTime currentTime = DateTime.Now;
            if (!Days.Contains(currentTime.DayOfWeek))
            {
                return null;
            }

            TimeOnly currentTimeOnly = TimeOnly.FromDateTime(currentTime);
            if (currentTimeOnly < From || currentTimeOnly > To)
            {
                return null;
            }

            DateTime start = currentTime.Date.Add(From.ToTimeSpan());
            DateTime end = currentTime.Date.Add(To.ToTimeSpan());

            TimeSpan timeSinceFrom = currentTimeOnly - From;
            TimeSpan intervalSpan = Interval.ToTimeSpan();
            int intervalsElapsed = (int)(timeSinceFrom.TotalMinutes / intervalSpan.TotalMinutes);

            start = start.AddMinutes(intervalsElapsed * intervalSpan.TotalMinutes);
            end = start.Add(intervalSpan);

            if (end > currentTime.Date.Add(To.ToTimeSpan()))
            {
                end = currentTime.Date.Add(To.ToTimeSpan());
            }

            return (start, end);
        }
    }

    public (DateTime Start, DateTime End) NextActiveInterval
    {
        get
        {
            DateTime currentTime = DateTime.Now;
            DateTime nextStart = currentTime;

            while (true)
            {
                if (!Days.Contains(nextStart.DayOfWeek))
                {
                    nextStart = nextStart.Date.AddDays(1) + From.ToTimeSpan();
                    continue;
                }

                TimeOnly nextStartTimeOnly = TimeOnly.FromDateTime(nextStart);
                if (nextStartTimeOnly < From)
                {
                    nextStart = nextStart.Date + From.ToTimeSpan();
                }
                else if (nextStartTimeOnly > To)
                {
                    nextStart = nextStart.Date.AddDays(1) + From.ToTimeSpan();
                    continue;
                }

                TimeSpan timeSinceFrom = nextStartTimeOnly - From;
                TimeSpan intervalSpan = Interval.ToTimeSpan();
                int intervalsToAdd = (int)Math.Ceiling(timeSinceFrom.TotalMinutes / intervalSpan.TotalMinutes);

                DateTime start = nextStart.Date + From.ToTimeSpan() + TimeSpan.FromMinutes(intervalsToAdd * intervalSpan.TotalMinutes);
                DateTime end = start.Add(intervalSpan);

                if (end > start.Date + To.ToTimeSpan())
                {
                    end = start.Date + To.ToTimeSpan();
                }

                return (start, end);
            }
        }
    }

    public DateTime NextPossibleRun
    {
        get
        {
            DateTime now = DateTime.Now;
            var activeInterval = ActiveInterval;

            if (activeInterval.HasValue)
            {
                var (start, end) = activeInterval.Value;
                var nextTime = now.Add(Interval.ToTimeSpan());

                if (nextTime <= end)
                {
                    return nextTime;
                }
            }

            var nextInterval = NextActiveInterval;
            return nextInterval.Start;
        }
    }
    
}