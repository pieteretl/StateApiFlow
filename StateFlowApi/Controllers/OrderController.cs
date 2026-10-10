using Microsoft.AspNetCore.Mvc;

namespace StateFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(ICoordinator coordinator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken  cancellationToken)
    {
        var result = await coordinator.StartAsync(new MessageData { Id = request.Id }, cancellationToken);
        return Ok(result);
    }
}

public class CreateOrderRequest
{
    public Guid Id { get; set; }
}