namespace Willhaben.Domain.Models;

public interface IScrapingScheduleSettings
{
    public ICollection<DayOfWeek> Days { get; set; }
    public TimeOnly From { get; set; }
    public TimeOnly To { get; set; }
    public Interval Interval { get; set; }
    public bool IsValidTimeFrom(TimeOnly from);
    public bool IsValidTimeTo(TimeOnly to);

    public (DateTime Start, DateTime End)? ActiveInterval { get; }
    public (DateTime Start, DateTime End) NextActiveInterval { get; }
    public DateTime NextPossibleRun { get; }
    
}