namespace StateFlowApi;

public interface IReceivedMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class ReceivedMessage(ILogger<ReceivedMessage> logger) : IReceivedMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        logger.LogInformation($"Received {messageData.Id}");
        await Task.Delay(1);
        return MessageState.Process;
        return MessageState.Differ;
        return MessageState.Isolate;
    }
}