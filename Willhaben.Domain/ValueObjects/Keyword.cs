using System.Text;
using System.Text.RegularExpressions;
using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public abstract class Keyword
{
    public  abstract string KeywordType { get; }
    private string _value { get; set; }
    public string Value
    {
        get => _value;  
        set
        {
            if (value is null)
            {
                throw new KeywordTooShortException();
            }
            value = value.Trim().ToLower();
            Validate(value);
            _value = value;
        }
    }
    

    public Keyword(string term)
    {
        Value = term;
    }


    private static void Validate(string value)
    {
        if (String.IsNullOrWhiteSpace(value) || value.Length < 1)
        {
            throw new KeywordTooShortException();
        }

        if (!HasOnlyValidChars(value))
        {
            throw new InvalidCharacterException("Word contains invalid characters");
        }
        
    }

    public static bool HasOnlyValidChars(string keyword)
    {
        string pattern = @"^[a-zA-Z0-9äöüßÄÖÜ]+$";  // Note: escaped double quote and hyphen
        return Regex.IsMatch(keyword, pattern);
    }


    public static void AddUnique<KType>(List<KType> keywords, KType keyword) where KType : Keyword
    {
        if (keywords.Any(k => k.Equals(keyword)))
        {
            throw new KeywordExistsException(keyword.Value);
        }
        keywords.Add(keyword);
    }

    public static List<KType> FromString<KType>(string input, Func<string, KType> factory) where KType : Keyword
    {
        if (string.IsNullOrEmpty(input))
        {
            // If the input is empty, return an empty list
            return new List<KType>();
        }

        var keywordList = new List<KType>();

        foreach (var keywordString in input.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var keyword = factory(keywordString.Trim());
            Keyword.AddUnique(keywordList, keyword);
        }

        return keywordList;
    }
    

    public new virtual  string ToString()
    {
        return _value;
    }
    
    
    public override bool Equals(object? obj)
    {
        if (obj is Keyword other)
        {
            return _value == other._value;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value);
    }

    public static bool operator ==(Keyword left, Keyword right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Keyword left, Keyword right) => !(left == right);
}

public class FuzzyKeyword : Keyword
{
    public FuzzyKeyword(string term) : base(term)
    {
    }

    public static IList<FuzzyKeyword> FromString(string input)
    {
        return Keyword.FromString(input, term => new FuzzyKeyword(term));
    }

    public override string KeywordType { get; } = "Fuzzy";

}
public class OmitKeyword: Keyword
{
    public OmitKeyword(string term) : base(term)
    {
    }
    public override string KeywordType { get; } = "Omit";
    public static OmitKeyword Create(string term) => new OmitKeyword(term);

    public static IList<OmitKeyword> FromString(string input)
    {
        return Keyword.FromString(input, term => new OmitKeyword(term));
    }


    public override string ToString()
    {
        return $"-{Value}"; 
    }
}
public class ExactKeyword: Keyword
{
    public ExactKeyword(string term) : base(term)
    {
    }
    public override string KeywordType { get; } = "Exact";

    public static IList<ExactKeyword> FromString(string input)
    {
        return Keyword.FromString(input, term => new ExactKeyword(term));
    }
    public override string ToString()
    {
        return $"%22{Value}%22"; 
    }
    public static ExactKeyword Create(string term) => new ExactKeyword(term);

}

