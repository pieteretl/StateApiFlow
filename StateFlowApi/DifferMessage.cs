namespace StateFlowApi;

public interface IDifferMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class DifferMessage : IDifferMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Process;
    }
}