using Willhaben.Domain.Models;

namespace Willhaben.Domain.Exceptions
{
    public class StateException : Exception
    {
        public StateException(string message) : base(message) { }
    }

    public class StateExistsException(string message) : KeywordException(message)
    {

        public StateExistsException(State.Zustand zustand) : this($"{zustand.ToString()} already exists.") { }
    }
}