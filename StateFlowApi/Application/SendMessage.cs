using StateFlowApi.Contracts;
using StateFlowApi.Infrastructure;

namespace StateFlowApi;

public interface ISendMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class SendMessage
(
    ILogger<SendMessage> logger,
    IS3Client s3Client,
    ITokeniseClient tokeniseClient,
    IDynamoDbClient dynamoDbClient,
    ISecretClient secretClient
) : ISendMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);

        // logger.LogInformation("Get the status of the message in the DB");
        // await dynamoDbClient.Get();
        
        logger.LogInformation("Hydrate the message data from S3");
        await s3Client.Get();
        
        logger.LogInformation("Hydrate the key from from secret manager");
        await secretClient.Get();
        
        logger.LogInformation("Map the message to a external client");
        
        logger.LogInformation("{Handler} with {MessageId}",
            nameof(SendMessage), ResolveMessageId(message));

        var success = Random.Shared.Next(2) == 0;
        
        await tokeniseClient.Send();
        
        var state = success ? MessageState.Finish : MessageState.Fail;

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