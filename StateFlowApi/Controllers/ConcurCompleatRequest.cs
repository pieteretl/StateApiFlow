namespace StateFlowApi.Controllers;

public class ConcurCompleatRequest
{
    public CircuitState CircuitState { get; set; }
}

public abstract record MessageBase
{
    public required MetaData MetaData { get; init; }
}

public sealed record MetaData
{
    public required string EventId { get; init; }
    public required string MessageId { get; init; }
    public required string EventType { get; init; }
    public required string EventSubType { get; init; }
    public required string Version { get; init; }
    public required string EntryPointId { get; init; }
    public required string ManagedGroupId { get; init; }
    public required string CorporateId { get; init; }
    public required string Tenant { get; init; }
    public required string ContextUri { get; init; }
    public required string OrganisationalUnitCode { get; init; }
    public required string IntegratorId { get; init; }
    public required DateTime TimeUtc { get; init; }
    public required string ConversationId { get; init; }
    public required string DeliveryStatusCallbackUri { get; init; }
    public required string S3ObjectKey { get; init; }
}

public sealed record ConcurCompleatMessage : MessageBase
{
    public required EventData EventData { get; init; }
}

public sealed record EventData
{
    public required string ItineraryId { get; init; }
    public required MessageIdentity MessageIdentity { get; init; }
    public MessageDependency? MessageDependency { get; init; }
}

public sealed record MessageIdentity
{
    public required string OperationType { get; init; }
    public required string Resource { get; init; }
    public required string ResourceId { get; init; }
}

public sealed record MessageDependency
{
    public required string MessageId { get; init; }
    public required string OperationType { get; init; }
    public required string Resource { get; init; }
    public required string ResourceId { get; init; }
}