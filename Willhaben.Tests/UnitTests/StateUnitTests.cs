using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Domain.Utils;

namespace Willhaben.Tests.UnitTests;

public class StateUnitTests
{
    [Fact]
    public void CreateState_Should_Succeed()
    {
        // Arrange
        var state = State.NEU;
        Assert.Equal(State.NEU, state);
        Assert.Equal(22,state.GetValue());
    }
    [Fact]
    public void GetCode_Should_HaveCodeForAllEnumValues()
    {
        foreach (State zustand in Enum.GetValues(typeof(State)))
        {
            Assert.True(zustand.GetValue() > 0);
        }
    }
    [Fact]
    public void AddState_Should_WorkCorrectly()
    {
        var states = new List<State>();
        foreach (State zustand in Enum.GetValues(typeof(State)))
        {
            states.AddUnique(zustand);
        }
        Assert.Equal(Enum.GetValues(typeof(State)).Length, states.Count);
        
    }
    [Fact]
    public void AddState_Should_ThrowOnDuplicateZustand()
    {
        var states = new List<State>();
        foreach (State zustand in Enum.GetValues(typeof(State)))
        {
            states.AddUnique(zustand);
        }
        Assert.Throws<EnumKeyExistsException<State>>(()=> states.AddUnique(State.NEU));
    }
    
}