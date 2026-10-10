namespace StateFlowApi;

public class Coordinator
(
    IReceivedMessage receivedMessage,
    IDifferMessage differMessage,
    IProcessMessage processMessage,
    IIsolateMessage isolateMessage,
    ICancelMessage cancelMessage,
    IFailMessage failMessage,
    IRetryMessage retryMessage,
    IRejectMessage rejectMessage,
    IStartMessage startMessage,
    ILogger<Coordinator> logger
) : ICoordinator
{
    public async Task<bool> StartAsync(MessageData messageData, CancellationToken cancellationToken)
    {
        var messageState = MessageState.Start;

        try
        {
            while (messageState is not MessageState.Success)
            {
                messageState = await TransitionAsync(messageState, messageData, cancellationToken);

                if (messageState is MessageState.Cancelled)
                {
                    logger.LogInformation("Message was cancelled for {MessageId}", messageData.Id);
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}", messageData.Id);
            throw;
        }

        logger.LogInformation("messageState {MessageState}", messageState);
        return true;
    }

    private async Task<MessageState> TransitionAsync(MessageState currentState, MessageData messageData, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return currentState switch
        {
            MessageState.Start => await startMessage.Process(messageData),
            MessageState.Reject => await rejectMessage.Process(messageData),
            MessageState.Differ => await differMessage.Process(messageData),
            MessageState.Received => await receivedMessage.Process(messageData),
            MessageState.Process => await processMessage.Process(messageData),
            MessageState.Isolate => await isolateMessage.Process(messageData),
            // MessageState.Cancelled => await cancelMessage.Process(messageData),
            MessageState.Fail => await failMessage.Process(messageData),
            MessageState.Retry => await retryMessage.Process(messageData),
            _ => MessageState.Success
        };
    }
}


public interface ICoordinator
{
    Task<bool> StartAsync(MessageData messageData, CancellationToken cancellationToken);
}

public class MessageData
{
    public Guid Id { get; set; }
}

