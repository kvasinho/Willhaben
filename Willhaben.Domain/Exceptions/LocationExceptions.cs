namespace Willhaben.Domain.Exceptions
{
    public class LocationException : Exception
    {
        public LocationException(string message) : base(message) { }
    }

    public class InvalidLocationException : LocationException
    {
        public InvalidLocationException(string location) 
            : base($"Location '{location}' is invalid") { }
    }

    public class LocationExistsException : LocationException
    {
        public LocationExistsException(string location) 
            : base($"Location '{location}' already exists") { }
    }
    public class LocationParentExistsException : LocationException
    {
        public LocationParentExistsException(string location) 
            : base($"The region of {location} exists already") { }
    }
    public class LocationChildExistsException : LocationException
    {
        public LocationChildExistsException(string location) 
            : base($"There is already a specific location in this region: {location}") { }
    }
    
}