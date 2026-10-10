using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface ICancelMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class CancelMessage : ICancelMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}