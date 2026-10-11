using StateFlowApi.Contracts;
using StateFlowApi.Infrastructure;

namespace StateFlowApi.Application;

public interface IFinishMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class FinishMessage(ILogger<StartMessage> logger, IDynamoDbClient dynamoDbClient) : IFinishMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("Starting {MessageId}", ResolveMessageId(message));
        logger.LogInformation("Finishing  storing the result in DB");
        await dynamoDbClient.Store();
        
        return MessageState.Done;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}
