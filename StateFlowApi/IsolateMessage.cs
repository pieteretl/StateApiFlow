namespace StateFlowApi;

public interface IIsolateMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class IsolateMessage(ILogger<IsolateMessage> logger) : IIsolateMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        logger.LogInformation($"Entering {nameof(IsolateMessage)} with {messageData.Id}");
        await Task.Delay(1);
        logger.LogInformation("Let pretend that this has been initiated manually");
        return MessageState.Process;
        return MessageState.Cancelled;
    }
}