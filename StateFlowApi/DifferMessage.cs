namespace StateFlowApi;

public interface IDifferMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class DifferMessage(ILogger<DifferMessage> logger) : IDifferMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        logger.LogInformation("DifferMessage processed with id: {id}, change the process time to 2min later", messageData.Id);
        return MessageState.Process;
    }
}