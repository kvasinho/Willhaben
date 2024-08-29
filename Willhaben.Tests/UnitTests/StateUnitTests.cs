using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Tests.UnitTests;

public class StateUnitTests
{
    [Fact]
    public void CreateState_Should_Succeed()
    {
        // Arrange
        var state = new State(StateType.NEU);
        Assert.Equal(StateType.NEU, state.Value);
        Assert.Equal(22,state.Code);
    }
    [Fact]
    public void GetCode_Should_HaveCodeForAllEnumValues()
    {
        foreach (StateType zustand in Enum.GetValues(typeof(StateType)))
        {
            var state = new State(zustand);
            Assert.True(state.Code > 0);
        }
    }
    [Fact]
    public void AddState_Should_WorkCorrectly()
    {
        var states = new List<State>();
        foreach (StateType zustand in Enum.GetValues(typeof(StateType)))
        {
            State.AddZustand(states,zustand);
        }
        
        Assert.Equal(Enum.GetValues(typeof(StateType)).Length, states.Count);
        
    }
    [Fact]
    public void AddState_Should_ThrowOnDuplicateZustand()
    {
        var states = new List<State>();
        foreach (StateType zustand in Enum.GetValues(typeof(StateType)))
        {
            State.AddZustand(states,zustand);
        }
        Assert.Throws<StateExistsException>(()=>State.AddZustand(states,StateType.NEU));
    }
    
}