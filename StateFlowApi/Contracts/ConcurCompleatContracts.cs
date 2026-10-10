namespace StateFlowApi.Contracts;

public class MessageEnvelope<TMessage> where TMessage : class
{
    public string Id { get; set; } = string.Empty;
    public TMessage Message { get; set; } = default!;
    public string MessageTypeIdentifier { get; set; } = string.Empty;
    public SQSMetadata SQSMetadata { get; set; } = new();
}

public class ConcurCompleatMessage
{
    public EventData EventData { get; set; } = new();
    public MetaData MetaData { get; set; } = new();
}

public class EventData
{
    public string ItineraryId { get; set; } = string.Empty;
    public MessageIdentity MessageIdentity { get; set; } = new();
    public MessageDependency MessageDependency { get; set; } = new();
}

public class MessageIdentity
{
    public string OperationType { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
}

public class MessageDependency
{
    public string MessageId { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
}

public class MetaData
{
    public string EventId { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string EventSubType { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string EntryPointId { get; set; } = string.Empty;
    public string ManagedGroupId { get; set; } = string.Empty;
    public string CorporateId { get; set; } = string.Empty;
    public string Tenant { get; set; } = string.Empty;
    public string ContextUri { get; set; } = string.Empty;
    public string OrganisationalUnitCode { get; set; } = string.Empty;
    public string IntegratorId { get; set; } = string.Empty;
    public DateTime TimeUtc { get; set; }
    public string ConversationId { get; set; } = string.Empty;
    public string DeliveryStatusCallbackUri { get; set; } = string.Empty;
    public string S3ObjectKey { get; set; } = string.Empty;
}

public class SQSMetadata
{
    public string MessageID { get; set; } = string.Empty;
    public string ReceiptHandle { get; set; } = string.Empty;
    public int ApproximateReceiveCount { get; set; }
}

