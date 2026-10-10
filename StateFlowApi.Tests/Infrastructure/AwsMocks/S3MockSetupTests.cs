using StateFlowApi.Tests.Infrastructure;
using StateFlowApi.Tests.TestFixtures;
using Xunit;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

[Collection("AWS Collection")]
public class S3MockSetupTests : IAsyncLifetime
{
    private readonly AwsFixture _fixture;
    private HttpClient? _httpClient;

    public S3MockSetupTests(AwsFixture fixture)
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
    public async Task MockGetObject_ShouldReturnContent()
    {
        // Arrange
        var bucketName = "test-bucket";
        var key = "test-key";
        var content = "test content";
        
        _fixture.AwsServices.S3.MockGetObject(bucketName, key, content);

        // Act
        var response = await _httpClient!.GetAsync($"/{bucketName}/{key}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(content, responseContent);
    }
}

