using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IRetryMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class RetryMessage(ILogger<RetryMessage> logger, ICircuitStateContext circuitStateContext) : IRetryMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        logger.LogInformation("Starting {MessageId} in {Handler}", ResolveMessageId(message), nameof(RetryMessage));
        await Task.Delay(1);
        
        if (circuitStateContext.State == CircuitState.Open)
        {
            return MessageState.Isolate;
        }
        
        return MessageState.Process;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}