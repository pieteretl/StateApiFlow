namespace StateFlowApi.Infrastructure;

public class DynamoDbClient : IDynamoDbClient
{
    public async Task<DbData> Get()
    {
        await Task.Delay(1);
        return await Task.FromResult(new DbData
            {
                Guid = Guid.NewGuid()
            }
        );
    }

    public async Task<DbData> StoreMetadataWithMessageStatus(MessageState receive)
    {
        await Task.Delay(1);
        return await Task.FromResult(new DbData
            {
                Guid = Guid.NewGuid()
            }
        );
    }

    public async Task Store()
    {
        await Task.Delay(1);
    }
}

public interface IDynamoDbClient
{
    Task<DbData> Get();
    Task<DbData> StoreMetadataWithMessageStatus(MessageState receive);
    Task Store();
}

public class DbData
{
    public Guid Guid { get; set; }
}