using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Domain.Utils;

namespace Willhaben.Tests.UnitTests;

public class LocationUnitTests
{
    [Theory]
    [InlineData(LocationType.BURGENLAND, 1)]
    [InlineData(LocationType.KAERNTEN, 2)]
    [InlineData(LocationType.WIEN, 900)]
    [InlineData(LocationType.KLAGENFURT, 201)]
    [InlineData(LocationType.KUFSTEIN, 705)]

    public void Location_Should_BeCreatedSuccessfully(LocationType @base, int @code)
    {
        // Arrange
        var location = new Location(@base);
        var defaultLocation = new Location();
        // Assert
        Assert.Equal(@code, location.Code);
        Assert.Equal(-1, defaultLocation.Code);
    }

    [Fact]
    public void AddLocation_Should_AddLocationsSuccessfully()
    {
        var locations = new List<Location>();
        
        //Add Location Objects
        Location.AddLocation(locations, new Location(LocationType.BURGENLAND));
        Location.AddLocation(locations, new Location(LocationType.WIEN));
        Location.AddLocation(locations, new Location(LocationType.WIEN_01));
        
        //Add LOcation enums and convert to objects in method 
        Location.AddLocation(locations, LocationType.IMST);
        Location.AddLocation(locations, LocationType.RUST);
        
        //Add location enum lists and convert in method 
        Location.AddLocations(locations, new List<LocationType>
        {
            LocationType.OBERWART,
            LocationType.WIEN_02
        });
        
        //Add Location object list 
        Location.AddLocations(locations, new List<Location>
        {
            new Location(LocationType.WIEN_03),
            new Location(LocationType.WIEN_04)
        });


        Assert.Equal(9,locations.Count);
        Assert.Equal(LocationType.BURGENLAND.GetValue(), locations[0].Code);
        Assert.Equal(LocationType.WIEN.GetValue(), locations[1].Code);
    }

    [Fact]
    public void AddLocation_ShouldThrowOnDuplicateLocation()
    {
        var locations = new List<Location>();
        var location1 = new Location(LocationType.IMST);
        var location2 = LocationType.IMST;
        
        Location.AddLocation(locations, location1);

        Assert.Throws<LocationExistsException>(() => Location.AddLocation(locations, location2));
    }
    [Fact]
    public void AddLocation_ShouldThrowOnAllLocation()
    {
        var locations = new List<Location>();
        
        Assert.Throws<InvalidLocationException>(() => Location.AddLocation(locations,LocationType.ALL));
        Assert.Throws<InvalidLocationException>(() => Location.AddLocation(locations,new Location()));
    }
    
    [Fact]
    public void AddLocations_ShouldThrowOnDuplicateLocation()
    {
        var existingLocations = new List<Location>();
    
        // Test with List<Location>
        Assert.Throws<LocationExistsException>(() => 
            Location.AddLocations(existingLocations, new List<Location>
            {
                new Location(LocationType.IMST),
                new Location(LocationType.IMST),
                new Location(LocationType.IMST)
            })
        );
    
        // Test with List<LocationType>
        Assert.Throws<LocationExistsException>(() => 
            Location.AddLocations(existingLocations, new List<LocationType>
            {
                LocationType.IMST,
                LocationType.IMST,
                LocationType.IMST
            })
        );
    }
    
    

}