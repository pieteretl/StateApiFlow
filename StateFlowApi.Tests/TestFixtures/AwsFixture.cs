using StateFlowApi.Tests.Infrastructure;
using Xunit;
using WireMock.Server;

namespace StateFlowApi.Tests.TestFixtures;

public class AwsFixture : IAsyncLifetime
{
    private WireMockServer? _server;
    public AwsServiceFactory AwsServices { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _server = WireMockServer.Start();
        AwsServices = new AwsServiceFactory(_server);
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        if (_server != null)
        {
            _server.Stop();
            _server.Dispose();
        }

        await Task.CompletedTask;
    }
}
