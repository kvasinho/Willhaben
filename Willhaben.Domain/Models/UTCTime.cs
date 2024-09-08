
using System;

namespace Willhaben.Domain.Models;
public class UTCTime : IComparable<UTCTime>
{
    private TimeOnly _utcTime;

    public override string ToString()
    {
        return _utcTime.ToString();
    }

    // Empty constructor: initializes with the current local time converted to UTC
    public UTCTime()
    {
        _utcTime = ConvertToUtc(DateTime.Now);
    }

    // Constructor to initialize the time in UTC format, with a provided local time
    public UTCTime(TimeOnly localTime)
    {
        _utcTime = ConvertToUtc(DateTime.Today.Add(localTime.ToTimeSpan()));
    }

    public UTCTime(DateTime dateTime, bool isUtc)
    {
        _utcTime = TimeOnly.FromDateTime(dateTime);
    }

    // Constructor to initialize directly with a UTC TimeOnly
    public UTCTime(TimeOnly utcTime, bool isUtc)
    {
        _utcTime = utcTime;  // Directly assign if it's already in UTC
    }

    // Property to get the UTC TimeOnly value
    public TimeOnly UtcTime
    {
        get { return _utcTime; }
    }

    // Static method to convert a local DateTime to UTC TimeOnly, using the local system time zone
    private static TimeOnly ConvertToUtc(DateTime localDateTime)
    {
        // Convert the local DateTime to UTC
        DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime);

        // Return just the time component as TimeOnly in UTC
        return TimeOnly.FromDateTime(utcDateTime);
    }
    
    

    // Optional: Method to update the time, ensuring it's always in UTC
    public void UpdateLocalTime(TimeOnly localTime)
    {
        _utcTime = ConvertToUtc(DateTime.Today.Add(localTime.ToTimeSpan()));
    }
    
    

    // Implementing IComparable<UTCTime> for sorting and comparisons
    public int CompareTo(UTCTime? other)
    {
        if (other == null)
            return 1; // Any instance is greater than null

        return _utcTime.CompareTo(other._utcTime);
    }
    
    private static TimeSpan GetUtcOffset()
    {
        DateTime localNow = DateTime.Now;
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        TimeSpan offset = localTimeZone.GetUtcOffset(localNow);

        return offset;
    }
    public static UTCTime FromLocalMinTime()
    {
        // Local minimum time is always TimeOnly.MinValue (00:00)
        TimeOnly localMinTime = TimeOnly.MinValue;

        // Determine the offset between local time and UTC
        TimeSpan offsetFromLocalToUtc = GetUtcOffset();

        // Convert local minimum time to UTC
        TimeOnly utcTime = localMinTime.Add(offsetFromLocalToUtc);
        
        return new UTCTime(utcTime, isUtc: true);
    }
    public static UTCTime FromLocalMaxTime()
    {
        // Local minimum time is always TimeOnly.MinValue (00:00)
        TimeOnly localMaxTime = TimeOnly.MaxValue;

        // Determine the offset between local time and UTC
        TimeSpan offsetFromLocalToUtc = GetUtcOffset();

        // Convert local minimum time to UTC
        TimeOnly utcTime = localMaxTime.Add(offsetFromLocalToUtc);
        
        return new UTCTime(utcTime, isUtc: true);
    }


    // Overload the == operator
    public static bool operator ==(UTCTime left, UTCTime right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left._utcTime == right._utcTime;
    }

    // Overload the != operator
    public static bool operator !=(UTCTime left, UTCTime right)
    {
        return !(left == right);
    }

    // Overload the < operator
    public static bool operator <(UTCTime left, UTCTime right)
    {
        return left._utcTime < right._utcTime;
    }

    // Overload the > operator
    public static bool operator >(UTCTime left, UTCTime right)
    {
        return left._utcTime > right._utcTime;
    }

    // Overload the <= operator
    public static bool operator <=(UTCTime left, UTCTime right)
    {
        return left._utcTime <= right._utcTime;
    }

    // Overload the >= operator
    public static bool operator >=(UTCTime left, UTCTime right)
    {
        return left._utcTime >= right._utcTime;
    }
    
    public static TimeSpan operator -(UTCTime left, UTCTime right)
    {
        DateTime leftDateTime = DateTime.Today.Add(left._utcTime.ToTimeSpan());
        DateTime rightDateTime = DateTime.Today.Add(right._utcTime.ToTimeSpan());

        return leftDateTime - rightDateTime;
    }

    // Override Equals method
    public override bool Equals(object? obj)
    {
        if (obj is UTCTime other)
        {
            return this == other;
        }

        return false;
            
    }

    // Override GetHashCode method
    public override int GetHashCode()
    {
        return _utcTime.GetHashCode();
    }
    
    

    
}