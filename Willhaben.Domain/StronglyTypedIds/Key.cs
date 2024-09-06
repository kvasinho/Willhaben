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
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Key cannot be null");
            }

            _value = value;
        }
    }

    public Key(string value)
    {
        Value = value;
    }


    public virtual bool Equals(Key? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException();
        }

        return _value == other._value;
    }
}