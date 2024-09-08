namespace Willhaben.Domain.Utils;

public static class TimeOnlyExtensions
{
    private static readonly TimeZoneInfo GermanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public static DateTime ConvertToUtc(TimeOnly localTime, DateTime date)
    {
        // Combine TimeOnly with a date to create a DateTime
        Console.WriteLine("ok");
        DateTime localDateTime = date.Date.Add(localTime.ToTimeSpan());
        Console.WriteLine("2");

        var t =  TimeZoneInfo.ConvertTimeToUtc(localDateTime, GermanTimeZone);
        Console.WriteLine("4");

        return t;
    }
}

