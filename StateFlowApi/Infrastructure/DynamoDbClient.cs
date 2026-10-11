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

    public async Task<DbData> StoreMetadataWithMessageStatus()
    {
        await Task.Delay(1);
        return await Task.FromResult(new DbData
            {
                Guid = Guid.NewGuid()
            }
        );
    }
}

public interface IDynamoDbClient
{
    Task<DbData> Get();
    Task<DbData> StoreMetadataWithMessageStatus();
}

public class DbData
{
    public Guid Guid { get; set; }
}