using Microsoft.AspNetCore.Mvc;

namespace StateFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(ICoordinator coordinator, ICircuitStateContext circuitStateContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken  cancellationToken)
    {
        circuitStateContext.State = request.CircuitState;
        var result = await coordinator.StartAsync(new MessageData { Id = request.Id, Type =  request.Type }, cancellationToken);
        return Ok(result);
    }
}

public class CreateOrderRequest
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "order";
    public CircuitState CircuitState { get; set; }
}

public enum CircuitState
{
    Closed,
    Open,
    HalfOpen
}

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

public interface ICircuitStateContext
{
    CircuitState State { get; set; }
}