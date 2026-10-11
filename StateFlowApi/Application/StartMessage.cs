using StateFlowApi.Contracts;

namespace StateFlowApi.Application;

public interface IStartMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class StartMessage(ILogger<StartMessage> logger) : IStartMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("Starting {MessageId}", ResolveMessageId(message));
        logger.LogInformation("Do some validation here");
        return MessageState.Receive;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}