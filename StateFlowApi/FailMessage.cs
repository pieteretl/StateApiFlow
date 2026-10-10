namespace StateFlowApi;

public interface IFailMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class FailMessage(ILogger<FailMessage> logger) : IFailMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        logger.LogInformation($"Starting {messageData.Id} in {nameof(FailMessage)}");
        await Task.Delay(1);
        return MessageState.Retry;
    }
}