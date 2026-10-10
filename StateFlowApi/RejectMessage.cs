namespace StateFlowApi;

public interface IRejectMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class RejectMessage(ILogger<RejectMessage> logger) : IRejectMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        logger.LogInformation($"Starting {messageData.Id} in {nameof(RejectMessage)}");
        logger.LogInformation("Move the message to isolated state");
        return MessageState.Isolate;
    }
}