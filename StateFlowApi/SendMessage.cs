using StateFlowApi.Contracts;

namespace StateFlowApi;

public interface ISendMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class SendMessage(ILogger<SendMessage> logger) : ISendMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);

        logger.LogInformation("Hydrate the message from S3");
        logger.LogInformation("Hydrate the key from from secret manager");
        logger.LogInformation("Map the message to a external client and send a message");
        
        logger.LogInformation("{Handler} with {MessageId}",
            nameof(SendMessage), ResolveMessageId(message));

        var success = Random.Shared.Next(2) == 0;

        var state = success ? MessageState.Success : MessageState.Fail;

        logger.LogInformation("Message result: {State}", state);

        return state;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}