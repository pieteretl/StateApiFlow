using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using System.Text.Json;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

public class DynamoDbMockSetup(WireMockServer server)
{
    public void MockPutItem(string tableName, Dictionary<string, object> item, int statusCode = 200)
    {
        server
            .Given(
                Request
                    .Create()
                    .WithPath("/")
                    .UsingPost()
                    .WithHeader("X-Amz-Target", new RegexMatcher("DynamoDB_.*PutItem"))
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "application/x-amz-json-1.0")
                    .WithBody("{}")
            );
    }

    public void MockGetItem(string tableName, string key, Dictionary<string, object> item, int statusCode = 200)
    {
        var responseBody = JsonSerializer.Serialize(new { Item = item });

        server
            .Given(
                Request
                    .Create()
                    .WithPath("/")
                    .UsingPost()
                    .WithHeader("X-Amz-Target", new RegexMatcher("DynamoDB_.*GetItem"))
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "application/x-amz-json-1.0")
                    .WithBody(responseBody)
            );
    }

    public void MockQuery(string tableName, List<Dictionary<string, object>> items, int statusCode = 200)
    {
        var responseBody = JsonSerializer.Serialize(new { Items = items, Count = items.Count });

        server
            .Given(
                Request
                    .Create()
                    .WithPath("/")
                    .UsingPost()
                    .WithHeader("X-Amz-Target", new RegexMatcher("DynamoDB_.*Query"))
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "application/x-amz-json-1.0")
                    .WithBody(responseBody)
            );
    }
}
