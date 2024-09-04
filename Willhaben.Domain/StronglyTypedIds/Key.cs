using Willhaben.Domain.Utils;

namespace Willhaben.Domain.StronglyTypedIds;

public record Key
{
    private  string _value { get; set; }
    public string Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (!value.HasOnlyLetters())
            {
                throw new ArgumentException("Key can only contain letters.");
            }

            _value = value;
        }
    }

    public Key(string value)
    {
        Value = value;
    }
    
}