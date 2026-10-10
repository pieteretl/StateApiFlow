namespace StateFlowApi;

public interface ICancelMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class CancelMessage : ICancelMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}