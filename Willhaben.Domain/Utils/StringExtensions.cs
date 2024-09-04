using System.Text.RegularExpressions;

namespace Willhaben.Domain.Utils;

public static class StringExtensions
{
    private static readonly Random Random = new Random();

    public static string GenerateRandomNDigitString(this string str, int digits = 4)
    {
        if (digits <= 0)
        {
            throw new ArgumentException("Number of digits must be greater than zero.", nameof(digits));
        }

        int minValue = (int)Math.Pow(10, digits - 1);
        int maxValue = (int)Math.Pow(10, digits) - 1;

        return str + "_" + Random.Next(minValue, maxValue + 1).ToString($"D{digits}");
    }
    public static bool HasOnlyLetters(this string keyword)
    {
        string pattern = @"^[a-zA-Z0-9äöüßÄÖÜ]+$";  // Note: escaped double quote and hyphen
        return Regex.IsMatch(keyword, pattern);
    }
}