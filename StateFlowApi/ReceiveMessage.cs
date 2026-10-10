using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IReceiveMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public sealed class ReceiveMessage(ILogger<ReceiveMessage> logger, ICircuitStateContext circuitStateContext) : IReceiveMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        logger.LogInformation("Received {MessageId} in {Handler}", ResolveMessageId(message), nameof(ReceiveMessage));
        await Task.Delay(1);
        
        if (circuitStateContext.State == CircuitState.Open || circuitStateContext.State == CircuitState.HalfOpen)
            return MessageState.Isolate;

        if (circuitStateContext.State == CircuitState.Closed)
        {
            logger.LogInformation("Can I process this order now?");

            if (!IsRefundMessage(message))
            {
                logger.LogInformation("This is a order message");
                logger.LogInformation("Process this order now");
                return MessageState.Process;
            }

            if (IsRefundMessage(message))
            {
                logger.LogInformation("This is a refund message");
                logger.LogInformation("I need to process this later");
                return MessageState.Differ;
            }
        }
        
        return MessageState.Unknown;
    }

    private static bool IsRefundMessage(ConcurCompleatMessage message)
    {
        var eventSubType = message.MetaData.EventSubType;
        var operationType = message.EventData.MessageIdentity.OperationType;

        return string.Equals(eventSubType, "refund", StringComparison.OrdinalIgnoreCase)
            || string.Equals(operationType, "refund", StringComparison.OrdinalIgnoreCase);
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}