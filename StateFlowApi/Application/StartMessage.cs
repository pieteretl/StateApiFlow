using StateFlowApi.Contracts;

namespace StateFlowApi.Application;

public interface IStartMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

/// <summary>
/// The goal of this type is whether the coordinator should accept this message.
/// Thus validation checks such as idempotency checks, or checking that the
/// message is well formed could be done here.
/// If it is then move to Receive state
/// </summary>
/// <param name="logger"></param>
public class StartMessage(ILogger<StartMessage> logger) : IStartMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        await Task.Delay(1);
        logger.LogInformation("Starting {MessageId}", ResolveMessageId(message));
        
        if (!IsValidMessage(message))
        {
            logger.LogInformation("Failed validation, cannot process message");
            return MessageState.Reject;
        }

        logger.LogInformation("Pass validation");
        return MessageState.Receive;
    }

    private bool IsValidMessage(ConcurCompleatMessage message)
    {
        return true;
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}