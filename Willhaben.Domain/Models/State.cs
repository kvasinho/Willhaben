using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class State
{
    public Zustand Value { get; set; }

    public int Code
    {
        get
        {
            if(Codes.TryGetValue(Value, out int val))
            {
                return val;
            }

            throw new StateException($"There is no code assigned to this State");
        }
    }

    public static Dictionary<Zustand, int> Codes = new Dictionary<Zustand, int>
    {
        { Zustand.NEU , 22},
        { Zustand.NEUWERTIG ,2546},
        { Zustand.GENERALÜBERHOLT ,5013256},
        { Zustand.GEBRAUCHT ,23},
        { Zustand.DEFEKT ,24},
        { Zustand.AUSSTELLUNGSSTÜCK ,2539}
    };

    public State(Zustand zustand)
    {
        Value = zustand;
    }

    public static void AddZustand(IList<State> states, Zustand state)
    {
        if (states.Any(s => s.Equals(state)))
        {
            throw new StateExistsException(state);
        }
        states.Add(new State(state));
    }

    public enum Zustand
    {
        NEU,
        NEUWERTIG,
        GENERALÜBERHOLT,
        GEBRAUCHT,
        DEFEKT,
        AUSSTELLUNGSSTÜCK,
    }
}