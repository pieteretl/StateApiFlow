namespace StateFlowApi;

public interface IRejectMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class RejectMessage : IRejectMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Isolate;
    }
}