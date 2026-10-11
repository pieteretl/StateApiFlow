using StateFlowApi.Contracts;

namespace StateFlowApi.OtherStates;

public interface IRejectMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class RejectMessage(ILogger<RejectMessage> logger) : IRejectMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("Move the message to isolated state");
        return MessageState.Isolate;
    }
}