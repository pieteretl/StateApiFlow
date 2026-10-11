using StateFlowApi.Contracts;

namespace StateFlowApi.Infrastructure;

public class SqsClient : ISqsClient
{
    public async Task<ConcurCompleatMessage> Poll()
    {
        await Task.Delay(1);
        return await Task.FromResult(new ConcurCompleatMessage
            {
                EventData = new EventData
                {
                    ItineraryId = Guid.NewGuid().ToString(),
                },
                MetaData = new MetaData
                {
                    ContextUri = "https://api.stateflow.com/contexts/1234567890",
                    EventId = Guid.NewGuid().ToString(),
                }
            }
        );
    }
}

public interface ISqsClient
{
    public Task<ConcurCompleatMessage> Poll();
}
