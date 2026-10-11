using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IIsolateMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class IsolateMessage(ILogger<IsolateMessage> logger) : IIsolateMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        logger.LogInformation("Entering {Handler} with {MessageId}", nameof(IsolateMessage), ResolveMessageId(message));
        await Task.Delay(1);
        logger.LogInformation("Let pretend that this has been initiated manually");
        return MessageState.Send;
        return MessageState.Cancel;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}