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
