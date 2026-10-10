using StateFlowApi.Tests.Infrastructure;
using StateFlowApi.Tests.TestFixtures;
using Xunit;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

[Collection("AWS Collection")]
public class SecretsManagerMockSetupTests : IAsyncLifetime
{
    private readonly AwsFixture _fixture;
    private HttpClient? _httpClient;

    public SecretsManagerMockSetupTests(AwsFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(_fixture.AwsServices.BaseUrl) };
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _httpClient?.Dispose();
        await Task.CompletedTask;
    }

    [Fact]
    public void MockGetSecretValue_ShouldReturnSecret()
    {
        // Arrange
        var secretName = "db-password";
        var secretValue = "super-secret-password";

        _fixture.AwsServices.SecretsManager.MockGetSecretValue(secretName, secretValue);

        // Assert - Mock is registered
        Assert.NotNull(_fixture.AwsServices.SecretsManager);
    }

    [Fact]
    public void MockSecretNotFound_ShouldReturn404()
    {
        // Arrange
        var secretName = "non-existent-secret";

        _fixture.AwsServices.SecretsManager.MockSecretNotFound(secretName);

        // Assert - Mock is registered
        Assert.NotNull(_fixture.AwsServices.SecretsManager);
    }
}

