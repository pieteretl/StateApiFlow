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
    private MessageState _messageState = MessageState.Start;
    private readonly Lock _lock = new();
    private int _failureCount = 0;
    
    public async Task<bool> StartAsync(MessageData messageData, CancellationToken cancellationToken)
    {
        try
        {
            while (_messageState is not MessageState.Success)
            {
                switch (_messageState)
                {
                    case MessageState.Reject:
                        _messageState = await rejectMessage.Process(messageData);
                        break;
                    case MessageState.Received:
                        _messageState = await receivedMessage.Process(messageData);
                        break;
                    case MessageState.Differ:
                        _messageState = await differMessage.Process(messageData);
                        break;
                    case MessageState.Process:
                        _messageState = await processMessage.Process(messageData);
                        break;
                    case MessageState.Isolate:
                        _messageState = await isolateMessage.Process(messageData);
                        break;
                    case MessageState.Cancelled:
                        _messageState = await cancelMessage.Process(messageData);
                        return false;
                    case MessageState.Fail:
                        _messageState = await failMessage.Process(messageData);
                        break;
                    case MessageState.Retry:
                        _messageState = await retryMessage.Process(messageData);
                        break;
                    case MessageState.Start:
                        _messageState = await startMessage.Process(messageData);
                        break;
                    default:
                        return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        logger.LogInformation($"messageState {_messageState}");
        return true;
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
