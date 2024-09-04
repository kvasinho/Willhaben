using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Domain.Utils;

namespace Willhaben.Tests.UnitTests;

public class LocationUnitTests
{
    [Theory]
    [InlineData(Location.BURGENLAND, 1)]
    [InlineData(Location.KAERNTEN, 2)]
    [InlineData(Location.WIEN, 900)]
    [InlineData(Location.KLAGENFURT, 201)]
    [InlineData(Location.KUFSTEIN, 705)]

    public void Location_Should_BeCreatedSuccessfully(Location @base, int @code)
    {

        Assert.Equal(@code, @base.GetValue());
    }

    [Fact]
    public void AddLocation_Should_AddLocationsSuccessfully()
    {
        var locations = new List<Location>();
        
        //Add Location Objects
        locations.AddUnique(Location.WIEN_13);
        locations.AddUnique(Location.WIEN_05);
        locations.AddUnique(Location.WIEN_01);
        
        Assert.Equal(3,locations.Count);
        Assert.Equal(Location.WIEN_13.GetValue(), locations[0].GetValue());
    }

    [Fact]
    public void AddLocation_ShouldThrowOnDuplicateLocation()
    {
        var locations = new List<Location>();
        var location1 = Location.IMST;
        var location2 = Location.IMST;
        
        locations.AddUnique(location1);

        Assert.Throws<EnumKeyExistsException<Location>>(() => locations.AddUnique(location2));
    }
    
    
    

}