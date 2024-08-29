
namespace Willhaben.Domain.Models
{
    public abstract class AbstractCodedEnumBaseClass<TEnum> where TEnum : Enum
    {
        public abstract TEnum Value { get; set; }

        public int Code => (int)(object)Value;

        protected abstract Exception KeyNotFoundException(TEnum value);

        public AbstractCodedEnumBaseClass(TEnum value)
        {
            Value = value;
        }
    }
}