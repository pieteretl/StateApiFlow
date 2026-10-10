namespace StateFlowApi;

public interface IIsolateMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class IsolateMessage : IIsolateMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Process;
        return MessageState.Cancelled;
    }
}