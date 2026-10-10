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
        _fixture.AwsServices.Reset();
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

    [Fact]
    public async Task MockGetJsonObject_ShouldReturnJsonPayload()
    {
        var bucketName = "test-bucket";
        var key = "messages/4530096.b2bmessage.json";
        var content = """
                      {
                        "data": {
                          "included": {
                            "orders": [
                              {
                                "orderId": "f888314a-549e-5ad6-bc83-424b1f740750",
                                "orderReference": "463085846061"
                              }
                            ]
                          }
                        }
                      }
                      """;

        _fixture.AwsServices.S3.MockGetJsonObject(bucketName, key, content);

        var response = await _httpClient!.GetAsync($"/{bucketName}/{key}");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("\"orderId\"", responseContent);
    }

    [Fact]
    public async Task MockGetJsonObjectFromFile_ShouldReturnFilePayload()
    {
        var bucketName = "test-bucket";
        var key = "messages/from-file.json";
        var fixtureFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Infrastructure",
            "AwsMocks",
            "test.b2bmessage.json");

        Assert.True(File.Exists(fixtureFilePath), $"Fixture file not found at {fixtureFilePath}");

        _fixture.AwsServices.S3.MockGetJsonObjectFromFile(bucketName, key, fixtureFilePath);

        var response = await _httpClient!.GetAsync($"/{bucketName}/{key}");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("\"settlementDetails\"", responseContent);
        Assert.Contains("\"orderReference\"", responseContent);
    }

    [Fact]
    public async Task MockObjectNotFound_ShouldReturn404AndNoSuchKey()
    {
        var bucketName = "test-bucket";
        var key = "messages/missing-object.json";

        _fixture.AwsServices.S3.MockObjectNotFound(bucketName, key);

        var response = await _httpClient!.GetAsync($"/{bucketName}/{key}");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/xml", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("NoSuchKey", responseContent);
        Assert.Contains(key, responseContent);
    }
}

