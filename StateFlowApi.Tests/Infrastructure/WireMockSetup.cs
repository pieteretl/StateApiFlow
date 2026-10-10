using Xunit;
using WireMock.Server;

namespace StateFlowApi.Tests.Infrastructure;

public class WireMockSetup : IAsyncLifetime
{
    private WireMockServer? _wireMockServer;
    public string BaseUrl => _wireMockServer?.Urls[0] ?? throw new InvalidOperationException("WireMock server not initialized");

    public async Task InitializeAsync()
    {
        _wireMockServer = WireMockServer.Start();
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        if (_wireMockServer != null)
        {
            _wireMockServer.Stop();
            _wireMockServer.Dispose();
        }

        await Task.CompletedTask;
    }

    public WireMockServer GetServer()
    {
        return _wireMockServer ?? throw new InvalidOperationException("WireMock server not initialized");
    }
}
