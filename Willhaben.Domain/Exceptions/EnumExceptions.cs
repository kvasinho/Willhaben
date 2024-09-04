
namespace Willhaben.Domain.Exceptions
{
    public class EnumException : Exception
    {
        public EnumException(string message) : base(message) { }
        
    }
    
    public class EnumKeyExistsException<TEnum> : EnumException where TEnum: Enum
    {
        public EnumKeyExistsException(TEnum eEnum) 
            : base($"Key '{eEnum}' already exists") { }
    }

    public class StringEnumConversionException: EnumException
    {
        public StringEnumConversionException(string value) 
            : base($"'{value}' is invalid.") { }
    }
    
}