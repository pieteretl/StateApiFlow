namespace StateFlowApi;

public interface IFailMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class FailMessage : IFailMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Retry;
    }
}