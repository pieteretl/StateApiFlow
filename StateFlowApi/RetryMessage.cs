namespace StateFlowApi;

public interface IRetryMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class RetryMessage : IRetryMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Process;
        return MessageState.Isolate;
    }
}