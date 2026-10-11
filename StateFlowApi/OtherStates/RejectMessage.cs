using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IRejectMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class RejectMessage(ILogger<RejectMessage> logger) : IRejectMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("Starting {MessageId} in {Handler}", ResolveMessageId(message), nameof(RejectMessage));
        logger.LogInformation("Move the message to isolated state");
        return MessageState.Isolate;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}