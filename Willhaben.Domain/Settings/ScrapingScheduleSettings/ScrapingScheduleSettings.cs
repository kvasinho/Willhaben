using System;
using System.Text.Json.Serialization;
using Willhaben.Domain.Models;
using Willhaben.Domain.Utils;
using Willhaben.Domain.Utils.Converters;



namespace Willhaben.Domain.Models
{
    
    public class ScrapingPeriod
    {
        public DayOfWeek StartDay { get; set; }
        public UTCTime StartTime { get; set; }
        public DayOfWeek EndDay { get; set; }
        public UTCTime EndTime { get; set; }
        
        //This method should add n seconds to the current date and then get the next active Date
        public DateTime GetNextPossibleActiveDateTimeAfterNSeoncds(int seconds)
        {
            
            //Console.WriteLine($"NEXT DATE IN ACTIVE PERIOD: {DateTime.UtcNow.AddSeconds(seconds).Add(DifferenceToNextFuturePeriodAfterNSeconds(seconds))} WIHT {seconds} SECOINDS");
            return DateTime.UtcNow.AddSeconds(seconds).Add(DifferenceToNextFuturePeriodAfterNSeconds(seconds));
        }

        //This is the next Datetime in which scraping is possible -> if DifferenceToNextFuturePeriod = 0 (period is active), then we add 0 seconds -> current UTCTIME. 
        public DateTime GetNextPossibleActiveDatetime  => DateTime.UtcNow.Add(DifferenceToNextFuturePeriod);

        //Adds n seconds to the next possible period -> if 0 then its still the active Period in n seconds
        public TimeSpan DifferenceToNextFuturePeriodAfterNSeconds(int seconds)
        {
            var now = DateTime.UtcNow.AddSeconds(seconds);
            var timeNow = new UTCTime(now, true);
            var dayNow = now.DayOfWeek;
                
            if (IsWithinPeriod(now))
            {
                return TimeSpan.Zero;
            }
            //IF ITS TODAY
            var delta = StartTime - timeNow; //returns an hourly delta from 
            if (StartDay == dayNow && StartTime >timeNow)
            {
                return delta;
            }

            var daysSkipped = 0;
            //If Starts the next day
            while (dayNow != StartDay)
            {
                dayNow = dayNow.GetNextDay();
                daysSkipped++;
            }

            return TimeSpan.FromDays(daysSkipped) + delta;
        }

        //This is the difference from the current Datetime to the next active period -> if 0 then current Date is in active Period
        [JsonIgnore]
        public TimeSpan DifferenceToNextFuturePeriod => DifferenceToNextFuturePeriodAfterNSeconds(0);

        public override string ToString()
        {
            if (StartDay == EndDay)
            {
                return $"{StartDay} {StartTime} to {EndTime}";
            }

            return $"{StartDay} {StartTime} to {EndDay} {EndTime}";
            
        }

        public bool IsWithinPeriod(DateTime dateTime)
        {
            var day = dateTime.DayOfWeek;
            var utcTime = new UTCTime(dateTime, true);
            if (StartDay == EndDay)
            {
                //Case: scraper only runs on a single day
                return day == StartDay && utcTime >= StartTime && utcTime <= EndTime;
            }
            //Case scraper has 2 days -> needs to be day1 & larger or day2 and smaller;
            return day == StartDay ? utcTime >= StartTime : utcTime <= EndTime;
        }

    }
    /// <summary>
    /// This Settings class is necessary in all scrapers.
    /// It contains the base settings for when to scrape.
    /// </summary>
    public class ScrapingScheduleSettings
    {
        [JsonConverter(typeof(SimplifyableCollectionConverter<DayOfWeekCollection, DayOfWeek>))]
        public DayOfWeekCollection Days { get; set; } = new();

        [JsonConverter(typeof(UtcTimeJsonConverter))]
        public UTCTime From { get; set; } = new(TimeOnly.MinValue);
        
        [JsonConverter(typeof(UtcTimeJsonConverter))]
        public UTCTime To { get; set; } = new(TimeOnly.MaxValue);
        public Interval Interval { get; set; } = Interval.M5;
        

        public bool IsValidTimeFrom(UTCTime from)
        {
            return from != To;
        }

        public bool IsValidTimeTo(UTCTime to)
        {
            return to != From;
        }
        [JsonIgnore]
        public List<ScrapingPeriod> ScrapingPeriods
        {
            get => GetScrapingPeriods();
        }
        public List<ScrapingPeriod> GetScrapingPeriods()
        {
            var periods = new List<ScrapingPeriod>();
            var intervalSpan = Interval.ToTimeSpan();

            foreach (var day in Days.Values)
            {
                UTCTime startDateTime = From;
                UTCTime endDateTime = To;

                if (endDateTime < startDateTime)
                {

                    DayOfWeek currentDay = day;
                    DayOfWeek nextDay = currentDay == DayOfWeek.Sunday ? DayOfWeek.Monday : currentDay + 1;

                    periods.Add(new ScrapingPeriod
                    {
                        StartDay = day,
                        StartTime = From,
                        EndDay = nextDay,
                        EndTime = To
                    });
                }
                else
                {
                    periods.Add(new ScrapingPeriod
                    {
                        StartDay = day,
                        StartTime = From,
                        EndDay = day,
                        EndTime = To
                    });

                }
            }

            return periods;
        }




        private DateTime GetDateTimeForDay(DayOfWeek dayOfWeek, UTCTime time)
        {
            var now = DateTime.UtcNow;
            var daysUntil = ((int)dayOfWeek - (int)now.DayOfWeek + 7) % 7;
            var targetDate = now.Date.AddDays(daysUntil);
            return targetDate + time.UtcTime.ToTimeSpan();
        }
        
        public bool IsDateWithinAnyPeriod(DateTime utcDate)
        {
            var periods = GetScrapingPeriods();
            foreach (var period in periods)
            {
                DateTime periodStart = GetDateTimeForDay(period.StartDay, period.StartTime);
                DateTime periodEnd = GetDateTimeForDay(period.EndDay, period.EndTime);

                // Adjust for periods that cross midnight
                if (period.EndDay < period.StartDay)
                {
                    periodEnd = periodEnd.AddDays(1);
                }

                if (utcDate >= periodStart && utcDate <= periodEnd)
                {
                    return true;
                }
            }
            return false;
        }
        
        [JsonIgnore]
        public ScrapingPeriod? CurrentActivePeriod
        {
            get
            {
                var utcNow = DateTime.UtcNow;
                
                foreach (var period in ScrapingPeriods)
                {
                    if (period.IsWithinPeriod(utcNow))
                    {
                        return period;
                    }
                }
            
                return null;
            }
        }
        

        [JsonIgnore]
        public ScrapingPeriod NextActivePeriod
        {
            get
            {
                var nextPeriod = ScrapingPeriods
                    .Where(p => p.DifferenceToNextFuturePeriod > TimeSpan.Zero)
                    .MinBy(p => p.DifferenceToNextFuturePeriod);
        
                if (nextPeriod == null)
                {
                    throw new InvalidOperationException("No future active period available.");
                }

                return nextPeriod;
            }
        }
        [JsonIgnore]
        public DateTime NextPossibleRuntime
        {
            get
            {
                var nextPeriod = ScrapingPeriods
                    .MinBy(p => p.DifferenceToNextFuturePeriod);
        
                if (nextPeriod == null)
                {
                    throw new InvalidOperationException("No future active period available.");
                }

                return nextPeriod.GetNextPossibleActiveDatetime;
            }
        }

        [JsonIgnore]
        public DateTime GetNextPossibleRunTimeAfterInterval 
        {
            get
            {
                //This finds and orders the periods by the different to the current time + interval;
                //If any period is still active, the DifferenceToNextFuturePeriodAfterNSeconds is 0 and therefore next period is the active period of the next run 
                //Since this field is called when a scraper is run, it can use the current datetime and check whether current datetime + interval seconds is still in an active interval
                var nextPeriod = ScrapingPeriods
                    .MinBy(p => p.DifferenceToNextFuturePeriodAfterNSeconds((int)Interval));

                //This is just there to satisfy the compiler.
                if (nextPeriod == null)
                {

                    throw new InvalidOperationException("No future active period available.");
                }
                //We need to return the nextPeriod datetime when we add n seconds to the curernt time
                return nextPeriod.GetNextPossibleActiveDateTimeAfterNSeoncds((int)Interval);
            }
        }
    }
}
