using StateFlowApi.Contracts;
using StateFlowApi.Infrastructure;

namespace StateFlowApi;

public interface IReceiveMessage
{
    Task<MessageState> Process(ConcurCompleatMessage message);
}

/// <summary>
/// Once validation checks have passed, the this step is store the message meta
/// data in the DB for processing and auditing.
/// This also checks whether the message is dependent on another message, such
/// as order must be received 1st, and handled before a refund.
/// Thus if a refund is found before a the order, then the message will be differed
/// </summary>
/// <param name="logger"></param>
/// <param name="circuitStateContext"></param>
/// <param name="dynamoDbClient"></param>
public sealed class ReceiveMessage(ILogger<ReceiveMessage> logger, ICircuitStateContext circuitStateContext, IDynamoDbClient dynamoDbClient) : IReceiveMessage
{
    public async Task<MessageState> Process(ConcurCompleatMessage message)
    {
        logger.LogInformation("Received {MessageId} in {Handler}", ResolveMessageId(message), nameof(ReceiveMessage));
        await Task.Delay(1);
        
        logger.LogInformation("Store this in DB, and give it a state of received");
        await dynamoDbClient.StoreMetadataWithMessageStatus(MessageState.Receive);

        if (IsDownstreamInError())
        {
            logger.LogInformation("Pretend that a downsteam service is down, we need to isolate the message for later processing");
            return MessageState.Isolate;
        }

        if (IsRefundMessage(message))
        {
            logger.LogInformation("This is a refund message");
            if (HasOrderBeenIssued(message))
            {
                logger.LogInformation("Order has been issued, process this now");
                return MessageState.Send;
            }
            logger.LogInformation("I need to process this later");
            return MessageState.Defer;
        }
            
        logger.LogInformation("This is a order message, process now");
        return MessageState.Send;
    }

    private bool IsDownstreamInError()
    {
        return circuitStateContext.State == CircuitState.Open || circuitStateContext.State == CircuitState.HalfOpen;
    }

    private bool HasOrderBeenIssued(ConcurCompleatMessage message)
    {
        return true;
    }

    /// <summary>
    /// TODO: We need the orderId in the contract
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static bool IsRefundMessage(ConcurCompleatMessage message)
    {
        var eventSubType = message.MetaData.EventSubType;
        var operationType = message.EventData.MessageIdentity.OperationType;

        return string.Equals(eventSubType, "refund", StringComparison.OrdinalIgnoreCase)
            || string.Equals(operationType, "refund", StringComparison.OrdinalIgnoreCase);
    }

    private static string ResolveMessageId(ConcurCompleatMessage message)
    {
        return string.IsNullOrWhiteSpace(message.MetaData.MessageId)
            ? message.MetaData.ConversationId
            : message.MetaData.MessageId;
    }
}