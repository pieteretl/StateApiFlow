using StateFlowApi.Controllers;

namespace StateFlowApi;

public interface IReceiveMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public sealed class ReceiveMessage(ILogger<ReceiveMessage> logger, ICircuitStateContext circuitStateContext) : IReceiveMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        logger.LogInformation($"Received {messageData.Id} in {nameof(ReceiveMessage)}");
        await Task.Delay(1);
        
        if (circuitStateContext.State == CircuitState.Open || circuitStateContext.State == CircuitState.HalfOpen)
            return MessageState.Isolate;

        if (circuitStateContext.State == CircuitState.Closed)
        {
            logger.LogInformation("Can I process this order now?");
        
            if (string.Equals(messageData.Type, "order", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("This is a order message");
                logger.LogInformation("Process this order now");
                return MessageState.Process;
            }

            if (string.Equals(messageData.Type, "refund", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("This is a refund message");
                logger.LogInformation("I need to process this later");
                return MessageState.Differ;
            }
        }
        
        return MessageState.Unknown;
    }
}