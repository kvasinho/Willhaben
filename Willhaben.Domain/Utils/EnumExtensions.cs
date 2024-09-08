using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Utils;

public static class EnumExtensions
{
    public static int GetEnumValue<TEnum>(TEnum enumValue) where TEnum : Enum
    {
        return Convert.ToInt32(enumValue);
    }
    public static void AddUnique<TEnum>(this ICollection<TEnum> collection, TEnum value) where TEnum: Enum
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(value);

        if (collection.Any(c => c.Equals(value)))
        {
            throw new EnumKeyExistsException<TEnum>(value);
        }
        collection.Add(value);
    }
    public static void AddUniqueIgnoreDuplicates<TEnum>(this ICollection<TEnum> collection, TEnum value) where TEnum: Enum
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(value);

        if (!collection.Any(c => c.Equals(value)))
        {
            collection.Add(value);
        }
    }
    public static List<T> GetAllValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    public static bool ContainsAllEnumValues<T>(this IEnumerable<T> list) where T : Enum
    {
        var allValues = GetAllValues<T>();
        return !allValues.Except(list).Any();
    }
    public static List<T> GetAllValuesBetween<T>(int from, int to) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Where(val => GetEnumValue(val) >= from & GetEnumValue(val) <= to).ToList();
    }
    

    public static List<T> GetAllValuesBetween<T>(this IEnumerable<T> values, int from, int to) where T : Enum
    {
        // Ensure the range is valid
        if (from > to)
        {
            throw new ArgumentException("The 'from' value must be less than or equal to the 'to' value.");
        }

        return values
            .Where(v => Convert.ToInt32(v) >= from && Convert.ToInt32(v) <= to)
            .ToList();
    }
    public static bool ContainsAllEnumValuesFromRange<T>(this IEnumerable<T> list, int from, int to) where T : Enum
    {
        var allValuesInRange = GetAllValuesBetween<T>(from, to);
        return !allValuesInRange.Except(list).Any();
    }
    public static bool Remove<T>(this List<T> list, T value) where T : Enum
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        return list.Remove(value);
    }
    public static void Remove<T>(this IEnumerable<T> list, T value) where T : Enum
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        // Attempt to remove the item
        list.Remove(value);
    }
    
    public static void AddRange<TEnum>(this ICollection<TEnum> collection, int from, int to) where TEnum : Enum
    {
        ArgumentNullException.ThrowIfNull(collection);

        var valuesInRange = GetAllValuesBetween<TEnum>(from, to);

        foreach (var value in valuesInRange)
        {
            collection.AddUnique(value);
        }
    }
    public static DayOfWeek GetNextDay(this DayOfWeek day)
    {
        return (DayOfWeek)(((int)day + 1) % 7);
    }
}