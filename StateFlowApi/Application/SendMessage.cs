using StateFlowApi.Contracts;
using StateFlowApi.Infrastructure;

namespace StateFlowApi.Application;

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
        logger.LogInformation("{Handler} with {MessageId}",
            nameof(SendMessage), ResolveMessageId(message));
        
        await Task.Delay(1);

        logger.LogInformation("Hydrate the message data from S3");
        await s3Client.Get();
        
        logger.LogInformation("Hydrate the key from from secret manager");
        await secretClient.Get();
        
        logger.LogInformation("Create the Request to Concur");
        var request = CreateRequest(message);

        logger.LogInformation("Send message to Concur");
        await tokeniseClient.Send();
        
        var success = Random.Shared.Next(2) == 0;
        var state = success ? MessageState.Finish : MessageState.Fail;

        logger.LogInformation("Store Concur result: {State}", state);
        await dynamoDbClient.StoreMetadataWithMessageStatus(state);

        return state;
    }

    private object CreateRequest(ConcurCompleatMessage message)
    {
        return new
        {
            message.MetaData.ConversationId,
            message.MetaData.MessageId,
        };
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}