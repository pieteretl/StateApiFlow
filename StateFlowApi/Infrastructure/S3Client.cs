namespace StateFlowApi.Infrastructure;

public class S3Client : IS3Client
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

public interface IS3Client
{
    public Task<DbData> Get();
}