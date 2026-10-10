using AutoFixture;
using StateFlowApi;
using Xunit;

namespace StateFlowApi.Tests;

public class CircuitStateContextTests
{
    private readonly CircuitStateContext _context = new();
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_ShouldInitializeWithClosedState()
    {
        // Assert
        Assert.Equal(CircuitState.Closed, _context.State);
    }

    [Theory]
    [InlineData(CircuitState.Closed)]
    [InlineData(CircuitState.Open)]
    [InlineData(CircuitState.HalfOpen)]
    public void State_Get_ShouldReturnCurrentState(CircuitState state)
    {
        // Arrange
        _context.State = state;

        // Act
        var result = _context.State;

        // Assert
        Assert.Equal(state, result);
    }

    [Theory]
    [InlineData(CircuitState.Closed)]
    [InlineData(CircuitState.Open)]
    [InlineData(CircuitState.HalfOpen)]
    public void State_Set_ShouldUpdateState(CircuitState state)
    {
        // Act
        _context.State = state;

        // Assert
        Assert.Equal(state, _context.State);
    }

    [Fact]
    public async Task ConcurrentReads_ShouldNotRaceCondition()
    {
        // Arrange
        _context.State = CircuitState.Open;
        var tasks = new List<Task>();
        var results = new List<CircuitState>();
        var lockObj = new object();

        // Act
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var state = _context.State;
                lock (lockObj)
                {
                    results.Add(state);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.All(results, state => Assert.Equal(CircuitState.Open, state));
    }

    [Fact]
    public async Task ConcurrentWrites_ShouldMaintainConsistency()
    {
        // Arrange
        var states = new[] { CircuitState.Closed, CircuitState.Open, CircuitState.HalfOpen };
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                foreach (var state in states)
                {
                    _context.State = state;
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert - Just verify that the final state is one of the valid values
        Assert.Contains(_context.State, states);
    }
}

