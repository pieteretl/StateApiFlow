using StateFlowApi.Contracts;
using StateFlowApi.Infrastructure;

namespace StateFlowApi.Application;

public interface IFinishMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

public class FinishMessage(ILogger<FinishMessage> logger, IDynamoDbClient dynamoDbClient) : IFinishMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("Finishing and storing the result in DB");
        await dynamoDbClient.StoreMetadataWithMessageStatus(MessageState.Done);
        
        return MessageState.Done;
    }
}
