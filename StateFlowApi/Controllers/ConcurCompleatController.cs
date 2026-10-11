using Microsoft.AspNetCore.Mvc;
using StateFlowApi.Application;
using StateFlowApi.Contracts;
using ContractMessage = StateFlowApi.Contracts.ConcurCompleatMessage;

namespace StateFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcurCompleatController(ICoordinator coordinator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MessageEnvelope<ContractMessage> request, CancellationToken cancellationToken)
    {
        if (request.Message?.MetaData is null)
        {
            return BadRequest("message.metaData is required");
        }

        var result = await coordinator.StartAsync(request.Message, cancellationToken);

        return Ok(result);
    }
}