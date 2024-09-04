
using System.Data;
using System.Runtime.CompilerServices;
using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models
{
    public abstract class AbstractCodedEnumBaseClass<TEnum> where TEnum : Enum
    {
        public abstract TEnum Value { get; set; }

        public int Code => (int)(object)Value;
        
        public AbstractCodedEnumBaseClass(TEnum value)
        {
            Value = value;
        }

        public static void AddUnique(IList<AbstractCodedEnumBaseClass<TEnum>> collection, AbstractCodedEnumBaseClass<TEnum> value)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            
            if (collection.Any(c => c.Value.Equals(value.Value)))
            {
                throw new DuplicateKeyException<TEnum>(value.Value);
            }
            collection.Add(value);
        }
        public static void AddUniqueCollection(IList<AbstractCodedEnumBaseClass<TEnum>> collection, IList<AbstractCodedEnumBaseClass<TEnum>> toAdd)
        {
            foreach (var elem in toAdd)
            {
                AddUnique(collection,elem);
            }
        }   
    }
}

namespace Willhaben.Domain.Exceptions
{
    public class EnumException<TEnum> : Exception where TEnum : Enum
    {
        public TEnum Value { get; }

        public EnumException(TEnum value, string message) : base(message)
        {
            Value = value;
        }
    }
    public class DuplicateKeyException<TEnum> : EnumException<TEnum> where TEnum : Enum
    {
        public DuplicateKeyException(TEnum value)
            : base(value, $"An item with the value {value} already exists in the collection.")
        {
        }

    }
}
