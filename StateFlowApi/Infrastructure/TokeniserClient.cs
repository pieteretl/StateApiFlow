namespace StateFlowApi.Infrastructure;

public class TokeniserClient : ITokeniseClient
{
    public async Task<DbData> Send()
    {
        await Task.Delay(1);
        return await Task.FromResult(new DbData
            {
                Guid = Guid.NewGuid()
            }
        );
    }
    
}

public interface ITokeniseClient
{
    public Task<DbData> Send();
}