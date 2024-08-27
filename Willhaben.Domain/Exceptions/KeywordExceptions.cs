namespace Willhaben.Domain.Exceptions
{
    public class KeywordException : Exception
    {
        public KeywordException(string message) : base(message) { }
    }

    public class InvalidCharacterException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Keyword contains invalid characters";

        public InvalidCharacterException() : this(DefaultMessage) { }
    }

    public class MultipleWordException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Keyword consists of multiple words. You may only separate words by -";

        public MultipleWordException() : this(DefaultMessage) { }
    }
    public class KeywordTooShortException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Keyword may consist of at least one character.";

        public KeywordTooShortException() : this(DefaultMessage) { }
    }    
    public class OmitAndExactException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Cannot set Omit and Exact to true";

        public OmitAndExactException() : this(DefaultMessage) { }
    }

    public class KeywordCollectionException(string message) : KeywordException(message)
    {
        private const string DefaultMessage = "Can only set to empty keyword search if Keywords collection is empty";

        public KeywordCollectionException() : this(DefaultMessage) { }
    }
}