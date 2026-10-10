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

                if (messageState == MessageState.Cancelled)
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
            MessageState.Reject => await rejectMessage.Process(messageData),
            MessageState.Received => await receivedMessage.Process(messageData),
            MessageState.Differ => await differMessage.Process(messageData),
            MessageState.Process => await processMessage.Process(messageData),
            MessageState.Isolate => await isolateMessage.Process(messageData),
            MessageState.Cancelled => await cancelMessage.Process(messageData),
            MessageState.Fail => await failMessage.Process(messageData),
            MessageState.Retry => await retryMessage.Process(messageData),
            MessageState.Start => await startMessage.Process(messageData),
            _ => MessageState.Success
        };
    }
}

public interface IStartMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class StartMessage(ILogger<StartMessage> logger) : IStartMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        logger.LogInformation($"Starting {messageData.Id}");
        return MessageState.Received;
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

public enum MessageState
{
    Reject,
    Received,
    Differ,
    Process,
    Isolate,
    Cancelled,
    Fail,
    Retry,
    Start,
    Success
}

public class RejectMessage : IRejectMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IRejectMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class ReceivedMessage(ILogger<ReceivedMessage> logger) : IReceivedMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        logger.LogInformation($"Received {messageData.Id}");
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IReceivedMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class DifferMessage : IDifferMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IDifferMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class ProcessMessage : IProcessMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IProcessMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class IsolateMessage : IIsolateMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IIsolateMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class CancelMessage : ICancelMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface ICancelMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class FailMessage : IFailMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IFailMessage
{
    Task<MessageState> Process(MessageData messageData);
}

public class RetryMessage : IRetryMessage
{
    public async Task<MessageState> Process(MessageData messageData)
    {
        await Task.Delay(1);
        return MessageState.Success;
    }
}

public interface IRetryMessage
{
    Task<MessageState> Process(MessageData messageData);
}
