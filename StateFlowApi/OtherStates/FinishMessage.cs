using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IFinishMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class FinishMessage : IFinishMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        return MessageState.Finish;
    }
}