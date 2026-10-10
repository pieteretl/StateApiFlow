using StateFlowApi.Controllers;

namespace StateFlowApi;

public interface IRetryMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class RetryMessage(ILogger<RetryMessage> logger, ICircuitStateContext circuitStateContext) : IRetryMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        logger.LogInformation($"Starting {messageData.Id} in {nameof(RetryMessage)}");
        await Task.Delay(1);
        
        if (circuitStateContext.State == CircuitState.Open)
        {
            return MessageState.Isolate;
        }
        
        return MessageState.Process;
    }
}