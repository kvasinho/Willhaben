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
        public abstract IReadOnlyList<Tenum> Simplyfy(IEnumerable<Tenum> values);
        public abstract IReadOnlyList<Tenum> Expand(IEnumerable<Tenum> simplifiedValues);

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
        public override IReadOnlyList<State> Simplyfy(IEnumerable<State> values)
        {
            return values.ContainsAllEnumValues() ? new List<State>() : Values;
        }
        public override IReadOnlyList<State> Expand(IEnumerable<State> values)
        {
            return values.Any() ? EnumExtensions.GetAllValues<State>() : Values;
        }
    }

    public class LocationCollection : SimplifyableEnumCollection<Location>
    {
        public override IReadOnlyList<Location> Simplyfy(IEnumerable<Location> values)
        {
            var simplifiedValeus = new List<Location>();
            if (values.ContainsAllEnumValuesFromRange(117223, 117245))
            {
                simplifiedValeus.AddRange(117223, 117245);
            }
            else
            {
                simplifiedValeus.Add(Location.WIEN);
            }
            
            if (values.ContainsAllEnumValuesFromRange(100, 199))
            {
                simplifiedValeus.AddRange(100, 199);
            }
            else
            {
                simplifiedValeus.Add(Location.BURGENLAND);
            }
            
            if (values.ContainsAllEnumValuesFromRange(200, 299))
            {
                simplifiedValeus.AddRange(200, 299);
            }
            else
            {
                simplifiedValeus.Add(Location.KAERNTEN);
            }
            
            if (values.ContainsAllEnumValuesFromRange(300, 399))
            {
                simplifiedValeus.AddRange(300, 399);
            }
            else
            {
                simplifiedValeus.Add(Location.NIEDEROESTERREICH);
            }
            
            if (values.ContainsAllEnumValuesFromRange(400, 499))
            {
                simplifiedValeus.AddRange(400, 499);
            }
            else
            {
                simplifiedValeus.Add(Location.OBEROESTERREICH);
            }
            
            if (values.ContainsAllEnumValuesFromRange(500, 599))
            {
                simplifiedValeus.AddRange(500, 599);
            }
            else
            {
                simplifiedValeus.Add(Location.SALZBURG);
            }
            
            if (values.ContainsAllEnumValuesFromRange(600, 699))
            {
                simplifiedValeus.AddRange(600, 699);
            }
            else
            {
                simplifiedValeus.Add(Location.STEIERMARK);
            }
            
            if (values.ContainsAllEnumValuesFromRange(700, 799))
            {
                simplifiedValeus.AddRange(1700, 799);
            }
            else
            {
                simplifiedValeus.Add(Location.TIROL);
            }
            
            if (values.ContainsAllEnumValuesFromRange(800, 899))
            {
                simplifiedValeus.AddRange(800, 899);
            }
            else
            {
                simplifiedValeus.Add(Location.VORARLBERG);
            }

            return simplifiedValeus;

        }

        public override IReadOnlyList<Location> Expand(IEnumerable<Location> simplifiedValues)
        {
            var expanded = new List<Location>();
            if (simplifiedValues.Contains(Location.WIEN))
            {
                expanded.AddRange<Location>(117223,117245);
            }

            if (simplifiedValues.Contains(Location.BURGENLAND))
            {
                expanded.AddRange(100,199);
            }

            if (simplifiedValues.Contains(Location.KAERNTEN))
            {
                expanded.AddRange(200,299);
            }
            if (simplifiedValues.Contains(Location.NIEDEROESTERREICH))
            {
                expanded.AddRange(300,399);
            }
            if (simplifiedValues.Contains(Location.OBEROESTERREICH))
            {
                expanded.AddRange(400,499);
            }
            if (simplifiedValues.Contains(Location.SALZBURG))
            {
                expanded.AddRange(500,599);
            }            
            if (simplifiedValues.Contains(Location.STEIERMARK))
            {
                expanded.AddRange(600,699);
            }
            if (simplifiedValues.Contains(Location.TIROL))
            {
                expanded.AddRange(700,799);
            }
            if (simplifiedValues.Contains(Location.VORARLBERG))
            {
                expanded.AddRange(800,899);
            }

            return expanded;

        }
    }
}