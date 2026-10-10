using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface IDifferMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class DifferMessage(ILogger<DifferMessage> logger) : IDifferMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("DifferMessage processed with id: {MessageId}, change the process time to 2min later", ResolveMessageId(message));
        return MessageState.Process;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}