using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class State: AbstractCodedEnumBaseClass<StateType>
{
    

    public State(StateType value) : base(value)
    {
    }

    public override StateType Value { get; set; }
    protected override Exception KeyNotFoundException(StateType value)
    {
        throw new InvalidStateException(value);
    }
    
    public static void AddZustand(IList<State> states, StateType state)
    {
        if (states.Any(s => s.Value == state))
        {
            throw new StateExistsException(state);
        }
        states.Add(new State(state));
    }
}

public enum StateType
{
    NEU = 22,
    NEUWERTIG = 2546,
    GENERALÜBERHOLT = 5013256,
    GEBRAUCHT = 23,
    DEFEKT = 24,
    AUSSTELLUNGSSTÜCK = 2539,
}