namespace StateFlowApi;

public interface IStartMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class StartMessage(ILogger<StartMessage> logger) : IStartMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        logger.LogInformation($"Starting {messageData.Id}");
        logger.LogInformation("Do some validation here");
        return MessageState.Received;
    }
}