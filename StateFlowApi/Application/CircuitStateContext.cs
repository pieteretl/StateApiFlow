namespace StateFlowApi;

public class CircuitStateContext : ICircuitStateContext
{
    private readonly Lock _lock = new();

    public CircuitState State
    {
        get
        {
            lock (_lock)
            {
                return field;
            }
        }
        set
        {
            lock (_lock)
            {
                field = value;
            }
        }
    } = CircuitState.Closed;
}

public enum CircuitState
{
    Closed,
    Open,
    HalfOpen
}

public interface ICircuitStateContext
{
    CircuitState State { get; set; }
}
