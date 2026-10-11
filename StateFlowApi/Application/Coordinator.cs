using StateFlowApi.Contracts;
using StateFlowApi.OtherStates;

namespace StateFlowApi.Application;

public class Coordinator
(
    IReceiveMessage receiveMessage,
    IDeferMessage deferMessage,
    ISendMessage sendMessage,
    IIsolateMessage isolateMessage,
    IFailMessage failMessage,
    IRetryMessage retryMessage,
    IRejectMessage rejectMessage,
    IStartMessage startMessage,
    IFinishMessage finishMessage,
    ILogger<Coordinator> logger
) : ICoordinator
{
    private readonly uint _maxFailures = 5;
    private readonly Lock _lock = new();
    private uint _messageCount = 0;
    
    
    public async ValueTask<bool> StartAsync(ConcurCompleatMessage message, CancellationToken cancellationToken)
    {
        var messageState = MessageState.Start;
        var messageId = ResolveMessageId(message);

        try
        {
            while (messageState is not MessageState.Done && _messageCount < _maxFailures)
            {
                messageState = await TransitionToNextState(messageState, message, cancellationToken);

                if (messageState is MessageState.Cancel)
                {
                    logger.LogInformation("Message was cancelled for {MessageId}", messageId);
                    return false;
                }
                
                // using (_lock.EnterScope())
                // {
                //     _messageCount++;
                // };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}", messageId);
            throw;
        }

        logger.LogInformation("messageState {MessageState}", messageState);
        return true;
    }

    private async Task<MessageState> TransitionToNextState(MessageState currentState, ConcurCompleatMessage message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return currentState switch
        {
            MessageState.Start => await startMessage.Process(message),
            MessageState.Reject => await rejectMessage.Process(message),
            MessageState.Defer => await deferMessage.Process(message),
            MessageState.Receive => await receiveMessage.Process(message),
            MessageState.Send => await sendMessage.Process(message),
            MessageState.Finish => await finishMessage.Process(message),
            MessageState.Isolate => await isolateMessage.Process(message),
            MessageState.Fail => await failMessage.Process(message),
            MessageState.Retry => await retryMessage.Process(message),
            _ => MessageState.Done
        };
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        if (!string.IsNullOrWhiteSpace(message.MetaData.MessageId))
        {
            return message.MetaData.MessageId;
        }

        if (!string.IsNullOrWhiteSpace(message.MetaData.ConversationId))
        {
            return message.MetaData.ConversationId;
        }

        return "unknown";
    }
}

public interface ICoordinator
{
    ValueTask<bool> StartAsync(ConcurCompleatMessage message, CancellationToken cancellationToken);
}

