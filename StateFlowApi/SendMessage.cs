namespace StateFlowApi;

public interface ISendMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class SendMessage(ILogger<SendMessage> logger) : ISendMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);

        logger.LogInformation("Hydrate the message from S3");
        logger.LogInformation("Hydrate the key from from secret manager");
        logger.LogInformation("Map the message to a external client and send a message");
        
        logger.LogInformation("{Handler} with {Id}",
            nameof(SendMessage), messageData.Id);

        var success = Random.Shared.Next(2) == 0;

        var state = success ? MessageState.Success : MessageState.Fail;

        logger.LogInformation("Message result: {State}", state);

        return state;
    }
}