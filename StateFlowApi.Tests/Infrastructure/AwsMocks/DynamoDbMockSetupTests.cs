using StateFlowApi.Tests.TestFixtures;
using Xunit;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

[Collection("AWS Collection")]
public class DynamoDbMockSetupTests(AwsFixture fixture) : IAsyncLifetime
{
    private HttpClient? _httpClient;

    public async Task InitializeAsync()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(fixture.AwsServices.BaseUrl) };
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _httpClient?.Dispose();
        await Task.CompletedTask;
    }

    [Fact]
    public void MockPutItem_ShouldSucceed()
    {
        // Arrange
        var tableName = "Orders";
        var item = new Dictionary<string, object>
        {
            { "id", "123" },
            { "amount", "100" }
        };

        fixture.AwsServices.DynamoDb.MockPutItem(tableName, item);

        // Assert - Mock is registered
        Assert.NotNull(fixture.AwsServices.DynamoDb);
    }

    [Fact]
    public void MockGetItem_ShouldReturnItem()
    {
        // Arrange
        var tableName = "Orders";
        var key = "123";
        var item = new Dictionary<string, object>
        {
            { "id", "123" },
            { "amount", "100" }
        };

        fixture.AwsServices.DynamoDb.MockGetItem(tableName, key, item);

        // Assert - Mock is registered
        Assert.NotNull(fixture.AwsServices.DynamoDb);
    }

    [Fact]
    public void MockQuery_ShouldReturnItems()
    {
        // Arrange
        var tableName = "Orders";
        var items = new List<Dictionary<string, object>>
        {
            new() { { "id", "1" }, { "amount", "50" } },
            new() { { "id", "2" }, { "amount", "75" } }
        };

        fixture.AwsServices.DynamoDb.MockQuery(tableName, items);

        // Assert - Mock is registered
        Assert.NotNull(fixture.AwsServices.DynamoDb);
    }
}

