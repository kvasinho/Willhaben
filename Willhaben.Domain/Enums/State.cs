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

    public class SimplifyableEnumCollection<TEnum> where TEnum : Enum
    {
        private List<TEnum> _values;
        public List<TEnum> Values => _values;
        public List<TEnum> SimplifiedValues => Simplyfy();
    
        public virtual List<TEnum> Expand(IEnumerable<TEnum> simplifiedValues)
        {
            //If it contains zero elements -> 
            return simplifiedValues.Any() ? EnumExtensions.GetAllValues<TEnum>() : simplifiedValues.ToList();
        }
        public virtual List<TEnum> Simplyfy()
        {
            //If all values are included in _values -> return an empty array.
            return _values.ContainsAllEnumValues() ? new List<TEnum>() : _values.ToList();
        }

        public void SetSimplifiedValues(List<TEnum> enums)
        {
            //Use this one when reading from json since they are simplified there.
            _values = enums.Count == 0 ? EnumExtensions.GetAllValues<TEnum>() : enums.ToList();
        }

        public void SetValues(ICollection<TEnum> enums)
        {
            _values = enums.ToList();
        }
        
        public void AddUnique(TEnum value)
        {
            if (_values.Contains(value))
            {
                throw new InvalidOperationException($"The value '{value}' already exists.");
            }
            _values.Add(value);
        }
    }
    /*
    public abstract class T<Tenum> where Tenum : Enum
    {
        private List<Tenum> _values;
    private List<Tenum> _simplifiedValues;
    private bool _isUpdating = false;  // Guard flag to prevent recursion

    public IReadOnlyList<Tenum> Values => _values.AsReadOnly();
    public IReadOnlyList<Tenum> SimplifiedValues => _simplifiedValues.AsReadOnly();

    protected SimplifyableEnumCollection()
    {
        _values = new List<Tenum>();
        _simplifiedValues = new List<Tenum>();
    }

    public void SetSimplifiedValues(IEnumerable<Tenum> simplifiedValues)
    {
        if (_isUpdating) return; // Prevent recursion

        _isUpdating = true;
        try
        {
            // Set simplified values directly and update _values based on Expand
            _simplifiedValues = simplifiedValues.ToList();
            _values = Expand(_simplifiedValues).ToList();
        }
        finally
        {
            _isUpdating = false;
        }
    }

    public virtual IReadOnlyList<Tenum> Simplyfy(IEnumerable<Tenum> values)
    {
        // Default simplification logic
        return values.ContainsAllEnumValues() ? new List<Tenum>() : values.ToList();
    }

    public virtual IReadOnlyList<Tenum> Expand(IEnumerable<Tenum> simplifiedValues)
    {
        // Default expansion logic
        return simplifiedValues.Any() ? EnumExtensions.GetAllValues<Tenum>() : simplifiedValues.ToList();
    }

    private void UpdateSimplifiedValues()
    {
        if (_isUpdating) return; // Avoid recursive updates
        _simplifiedValues = Simplyfy(_values).ToList();
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
    */

    public class StateCollection: SimplifyableEnumCollection<State> { }
    
    public class DayOfWeekCollection: SimplifyableEnumCollection<DayOfWeek> { }

    public class LocationCollection : SimplifyableEnumCollection<Location>
    {
        private static void SimplifyLocationValues(
            List<Location> simplifiedLocations, 
            List<Location> locations,
            Location region,
            int startRange,
            int endRange
            )
        {
            var locationsInRange = locations.GetAllValuesBetween(startRange, endRange);
            if (locations.ContainsAllEnumValuesFromRange(startRange, endRange))
            {
                simplifiedLocations.Add(region);
            }
            else if(locations.Contains(region))
            {
                simplifiedLocations.Add(region);
            }
            else
            {
                simplifiedLocations.AddRange(locationsInRange);
            }
        }

        public static void ExpandFromExistingRegion(
            List<Location> expandedLocations, //the list to expand in 
            List<Location> locations, //the original list
            Location region,
            int startRange,
            int endRange
        )
        {
            if (locations.Contains(region))
            {
                expandedLocations.AddRange(startRange,endRange);
                locations.Remove(Location.WIEN);
            }
        }
        public override List<Location> Simplyfy()
        {
            var simplifiedValues = new List<Location>();
            SimplifyLocationValues(simplifiedValues, Values, Location.WIEN, 117223, 117245);
            SimplifyLocationValues(simplifiedValues, Values, Location.BURGENLAND, 100, 199);
            SimplifyLocationValues(simplifiedValues, Values, Location.KÄRNTEN, 200, 299);
            SimplifyLocationValues(simplifiedValues, Values, Location.NIEDERÖSTERREICH, 300, 399);
            SimplifyLocationValues(simplifiedValues, Values, Location.OBERÖSTERREICH, 400, 499);
            SimplifyLocationValues(simplifiedValues, Values, Location.SALZBURG, 500, 599);
            SimplifyLocationValues(simplifiedValues, Values, Location.STEIERMARK, 600, 699);
            SimplifyLocationValues(simplifiedValues, Values, Location.TIROL, 700, 799);
            SimplifyLocationValues(simplifiedValues, Values, Location.VORARLBERG, 800, 899);
            SimplifyLocationValues(simplifiedValues, Values, Location.ANDERE_LÄNDER, -200, -2);

            return simplifiedValues;
        }
        
        public override List<Location> Expand(IEnumerable<Location> simplifiedValues)
        {
            var expanded = new List<Location>();
            ExpandFromExistingRegion(expanded, Values, Location.WIEN, 117223,117245);
            ExpandFromExistingRegion(expanded, Values, Location.BURGENLAND, 100,199);
            ExpandFromExistingRegion(expanded, Values, Location.KÄRNTEN, 200,299);
            ExpandFromExistingRegion(expanded, Values, Location.NIEDERÖSTERREICH, 300,399);
            ExpandFromExistingRegion(expanded, Values, Location.OBERÖSTERREICH, 400,499);
            ExpandFromExistingRegion(expanded, Values, Location.SALZBURG, 500,599);
            ExpandFromExistingRegion(expanded, Values, Location.STEIERMARK, 600,699);
            ExpandFromExistingRegion(expanded, Values, Location.TIROL, 700,799);
            ExpandFromExistingRegion(expanded, Values, Location.VORARLBERG, 800,899);
            ExpandFromExistingRegion(expanded, Values, Location.ANDERE_LÄNDER, -200,-2);

            foreach (var location in simplifiedValues)
            {
                expanded.AddUniqueIgnoreDuplicates(location);
            }
            return expanded;

        }
    }
}