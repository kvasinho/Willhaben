using System;
using System.Collections.Generic;
using System.Linq;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Utils;

namespace Willhaben.Domain.Models
{
    public enum State
    {
        NEU = 22,
        NEUWERTIG = 2546,
        GENERALÜBERHOLT = 5013256,
        GEBRAUCHT = 23,
        DEFEKT = 24,
        AUSSTELLUNGSSTÜCK = 2539,
    }

    public abstract class SimplifyableEnumCollection<Tenum> where Tenum : Enum
    {
        private List<Tenum> _values;
        private List<Tenum> _simplifiedValues;

        public IReadOnlyList<Tenum> Values => _values.AsReadOnly();
        public IReadOnlyList<Tenum> SimplifiedValues => _simplifiedValues.AsReadOnly();

        protected SimplifyableEnumCollection()
        {
            _values = new List<Tenum>();
            _simplifiedValues = new List<Tenum>();
        }

        // Abstract methods to be implemented in derived classes
        public virtual IReadOnlyList<Tenum> Simplyfy(IEnumerable<Tenum> values)
        {
            return values.ContainsAllEnumValues() ? new List<Tenum>() : Values;
        }
        public virtual IReadOnlyList<Tenum> Expand(IEnumerable<Tenum> values)
        {
            return values.Any() ? EnumExtensions.GetAllValues<Tenum>() : Values;
        }

        public void AddUnique(Tenum value)
        {
            if (_values.Contains(value))
            {
                throw new InvalidOperationException($"The value '{value}' already exists.");
            }

            _values.Add(value);
            UpdateSimplifiedValues();
        }

        public void Remove(Tenum value)
        {
            if (_values.Remove(value))
            {
                UpdateSimplifiedValues();
            }
            else
            {
                throw new InvalidOperationException($"The value '{value}' does not exist.");
            }
        }

        public void SetSimplifiedValues(IEnumerable<Tenum> simplifiedValues)
        {
            _simplifiedValues = simplifiedValues.ToList();
            _values = Expand(_simplifiedValues).ToList();
        }

        private void UpdateSimplifiedValues()
        {
            _simplifiedValues = Simplyfy(_values).ToList();
        }
        
        public  void FromArray(Tenum[] values)
        {
            foreach (var value in values)
            {
                AddUnique(value);
            }
        }
        public  void FromList(List<Tenum> values)
        {
            foreach (var value in values)
            {
                AddUnique(value);
            }
        }



        public  Tenum[] ToArray()
        {
            return SimplifiedValues.ToArray();
        }
        public  List<Tenum> ToList()
        {
            return SimplifiedValues.ToList();
        }
    }

    public class StateCollection: SimplifyableEnumCollection<State>
    {
        /*
        public override IReadOnlyList<State> Simplyfy(IEnumerable<State> values)
        {
            return values.ContainsAllEnumValues() ? new List<State>() : Values;
        }
        public override IReadOnlyList<State> Expand(IEnumerable<State> values)
        {
            return values.Any() ? EnumExtensions.GetAllValues<State>() : Values;
        }
        */
    }
    
    public class DayOfWeekCollection: SimplifyableEnumCollection<DayOfWeek>
    {
        /*
        public override IReadOnlyList<DayOfWeek> Simplyfy(IEnumerable<DayOfWeek> values)
        {
            return values.ContainsAllEnumValues() ? new List<DayOfWeek>() : Values;
        }
        public override IReadOnlyList<DayOfWeek> Expand(IEnumerable<DayOfWeek> values)
        {
            return values.Any() ? EnumExtensions.GetAllValues<DayOfWeek>() : Values;
        }
        */
    }

    public class LocationCollection : SimplifyableEnumCollection<Location>
    {
        public override IReadOnlyList<Location> Simplyfy(IEnumerable<Location> values)
        {
            var simplifiedValeus = new List<Location>();
            if (values.ContainsAllEnumValuesFromRange(117223, 117245))
            {
                simplifiedValeus.Add(Location.WIEN);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(117223,117245));
            }
            
            if (values.ContainsAllEnumValuesFromRange(100, 199))
            {
                simplifiedValeus.Add(Location.BURGENLAND);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(100,199));
            }
            
            if (values.ContainsAllEnumValuesFromRange(200, 299))
            {
                simplifiedValeus.Add(Location.KAERNTEN);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(200,299));
            }
            
            if (values.ContainsAllEnumValuesFromRange(300, 399))
            {
                simplifiedValeus.Add(Location.NIEDEROESTERREICH);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(300,399));
            }
            
            if (values.ContainsAllEnumValuesFromRange(400, 499))
            {
                simplifiedValeus.Add(Location.OBEROESTERREICH);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(400,499));
            }
            
            if (values.ContainsAllEnumValuesFromRange(500, 599))
            {
                simplifiedValeus.Add(Location.SALZBURG);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(500,599));
            }
            
            if (values.ContainsAllEnumValuesFromRange(600, 699))
            {
                simplifiedValeus.Add(Location.STEIERMARK);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(600,699));
            }
            
            if (values.ContainsAllEnumValuesFromRange(700, 799))
            {
                simplifiedValeus.Add(Location.TIROL);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(700,799));
            }
            
            if (values.ContainsAllEnumValuesFromRange(800, 899))
            {
                simplifiedValeus.Add(Location.VORARLBERG);
            }
            else
            {
                simplifiedValeus.AddRange(values.GetAllValuesBetween(800,899));
            }

            return simplifiedValeus;

        }

        public override IReadOnlyList<Location> Expand(IEnumerable<Location> simplifiedValues)
        {
            var expanded = new List<Location>();
            if (simplifiedValues.Contains(Location.WIEN))
            {
                expanded.AddRange(117223,117245);
                simplifiedValues.Remove(Location.WIEN);
            }

            if (simplifiedValues.Contains(Location.BURGENLAND))
            {
                expanded.AddRange(100,199);
                simplifiedValues.Remove(Location.BURGENLAND);

            }

            if (simplifiedValues.Contains(Location.KAERNTEN))
            {
                expanded.AddRange(200,299);
                simplifiedValues.Remove(Location.KAERNTEN);

            }
            if (simplifiedValues.Contains(Location.NIEDEROESTERREICH))
            {
                expanded.AddRange(300,399);
                simplifiedValues.Remove(Location.NIEDEROESTERREICH);

            }
            if (simplifiedValues.Contains(Location.OBEROESTERREICH))
            {
                expanded.AddRange(400,499);
                simplifiedValues.Remove(Location.OBEROESTERREICH);

            }
            if (simplifiedValues.Contains(Location.SALZBURG))
            {
                expanded.AddRange(500,599);
                simplifiedValues.Remove(Location.SALZBURG);

            }            
            if (simplifiedValues.Contains(Location.STEIERMARK))
            {
                expanded.AddRange(600,699);
                simplifiedValues.Remove(Location.STEIERMARK);

            }
            if (simplifiedValues.Contains(Location.TIROL))
            {
                expanded.AddRange(700,799);
                simplifiedValues.Remove(Location.TIROL);

            }
            if (simplifiedValues.Contains(Location.VORARLBERG))
            {
                expanded.AddRange(800,899);
                simplifiedValues.Remove(Location.VORARLBERG);

            }
            
            foreach (var location in simplifiedValues)
            {
                expanded.AddUniqueIgnoreDuplicates(location);
            }

            return expanded;

        }
    }
}