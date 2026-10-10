namespace StateFlowApi;

public interface IProcessMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class ProcessMessage(ILogger<ProcessMessage> logger) : IProcessMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        logger.LogInformation($"Processing {messageData.Id}");
        return MessageState.Success;
        return MessageState.Fail;
    }
}