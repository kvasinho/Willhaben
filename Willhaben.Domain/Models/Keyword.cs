using System.Text;
using System.Text.RegularExpressions;
using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class Keyword
{
    private bool _isOmitKeyword { get; set;}

    public bool IsOmitKeyword
    {
        get => _isOmitKeyword;
        set
        {
            if (_isExactKeyword && value)
            {
                throw new OmitAndExactException();
            }

            _isOmitKeyword = value;
        }
    }
    
    private bool _isExactKeyword { get; set; }
    public bool IsExactKeyword
    {
        get => _isExactKeyword;
        set
        {
            if (_isOmitKeyword && value)
            {
                throw new OmitAndExactException();
            }

            _isExactKeyword = value;
        }
    }
    private string _term { get; set; }
    public string Term
    {
        get => _term;
        set
        {
            Validate(value);
            _term = value;
        }
    }

    public Keyword(string term, bool isOmitKeyword = false, bool isExactKeyword = false)
    {
        if (isExactKeyword && isOmitKeyword)
        {
            throw new OmitAndExactException();
        }

        Term = term;
        IsOmitKeyword = isOmitKeyword;
        IsExactKeyword = isExactKeyword;
    }


    private static void Validate(string value)
    {
        if (String.IsNullOrWhiteSpace(value) || value.Length < 1)
        {
            throw new KeywordTooShortException();
        }
        
        /*
        // Can only be a single word
        if (!IsSingleWord(value))
        {
            throw new MultipleWordException("Only a single word is allowed");
        }
        */
        
        // Can only have valid characters
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

    public static bool IsSingleWord(string keyword)
    {
        string pattern = @"^[a-zA-Z0-9äöüß]+(-[a-zA-Zäöüß]+)?$";
        return Regex.IsMatch(keyword, pattern);
    }
    

    public override string ToString()
    {
        if (IsExactKeyword)
        {
            return $"%22{_term}%22"; 
        }
        if (IsOmitKeyword)
        {
            return $"-{_term}"; 
        }
        return _term;
    }
    
    public static string DisplayKeywordList(IEnumerable<Keyword> keywords)
    {
        var keywordStrings = keywords
            .Select(keyword => keyword.ToString())
            .Where(str => !string.IsNullOrEmpty(str))
            .ToList();

        return string.Join("%20", keywordStrings);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Keyword other)
        {
            return _term == other._term &&
                   IsOmitKeyword == other.IsOmitKeyword &&
                   IsExactKeyword == other.IsExactKeyword;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_term, IsOmitKeyword, IsExactKeyword);
    }

    public static bool operator ==(Keyword left, Keyword right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Keyword left, Keyword right) => !(left == right);
}

