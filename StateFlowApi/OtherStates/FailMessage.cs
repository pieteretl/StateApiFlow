using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IFailMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class FailMessage(ILogger<FailMessage> logger) : IFailMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        logger.LogInformation("Starting {MessageId} in {Handler}", ResolveMessageId(message), nameof(FailMessage));
        await Task.Delay(1);
        return MessageState.Retry;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}