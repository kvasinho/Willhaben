using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Tests.UnitTests;

public class LocationUnitTests
{
    [Theory]
    [InlineData("burgenland", 1)]
    [InlineData("kärnten", 2)]
    [InlineData("wien", 900)]
    [InlineData("klagenfurt", 201)]
    [InlineData("kufstein", 705)]

    public void Location_Should_BeCreatedSuccessfully(string @base, int @code)
    {
        // Arrange
        var category = new Location(@base);
        // Assert
        Assert.Equal(@code, category.Code);
    }
    [Theory]
    [InlineData("brgenland")]
    [InlineData("")]
    [InlineData(".asd")]
    public void Location_Should_ThrowOnInvalidLocation(string @base)
    {
        Assert.Throws<InvalidLocationException>(()=>new Location(@base));
    }

    [Fact]
    public void AddLocation_Should_AddLocationsSuccessfully()
    {
        var locations = new List<Location>();
        var l1 = new Location("burgenland");
        var l2 = new Location("wien");
        var l3 = new Location("01");

        Location.AddLocation(locations, l1);
        Location.AddLocation(locations, l2);
        Location.AddLocation(locations, l3);


        Assert.Equal(3,locations.Count);
        Assert.Equal("burgenland", locations[0].Value);
        Assert.Equal("wien", locations[1].Value);
        Assert.Equal("01", locations[2].Value);
    }

    [Fact]
    public void AddLocation_ShouldThrowOnDuplicateLocation()
    {
        var locations = new List<Location>();
        var location1 = new Location("burgenland");
        var location2 = new Location("burgenland");

        Location.AddLocation(locations, location1);

        Assert.Throws<LocationExistsException>(() => Location.AddLocation(locations, location2));
    }
    
    [Fact]
    public void AddLocations_ShouldThrowOnDuplicateLocation()
    {
        var locations = new List<Location>();
        var addins = new List<Location>
        {
            new Location("burgenland"),
            new Location("burgenland"),
            new Location("01"),

        };
        
        Assert.Throws<LocationExistsException>(() => Location.AddLocations(locations, addins));
    }
    [Fact]
    public void AddLocation_Should_AddLocationsListSuccessfully()
    {
        var locations = new List<Location>
        {
            new Location("burgenland"),
            new Location("wien"),
            new Location("01"),
        };
        var addins = new List<Location>
        {
            new Location("kärnten"),
            new Location("02"),
            new Location("klagenfurt"),

        };
        Location.AddLocations(locations,addins);


        Assert.Equal(6,locations.Count);
        Assert.Equal("burgenland", locations[0].Value);
        Assert.Equal("wien", locations[1].Value);
        Assert.Equal("01", locations[2].Value);
        Assert.Equal("kärnten", locations[3].Value);
        Assert.Equal("02", locations[4].Value);
        Assert.Equal("klagenfurt", locations[5].Value);

    }
    

}