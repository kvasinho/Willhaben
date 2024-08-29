using Willhaben.Domain.Models;

namespace Willhaben.Domain.Exceptions
{
    public class StateException : Exception
    {
        public StateException(string message) : base(message) { }
    }
    public class InvalidStateException : StateException
    {
        public InvalidStateException(StateType state) 
            : base($"State '{state}' is invalid") { }
    }

    public class StateExistsException(string message) : KeywordException(message)
    {

        public StateExistsException(StateType state) : this($"{state.ToString()} already exists.") { }
    }
}