namespace StateFlowApi.Infrastructure;

public class SecretClient : ISecretClient
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
}

public interface ISecretClient
{
    public Task<DbData> Get();
}