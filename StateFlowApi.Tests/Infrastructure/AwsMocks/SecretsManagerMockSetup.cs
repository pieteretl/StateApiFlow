using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using System.Text.Json;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

public class SecretsManagerMockSetup(WireMockServer server)
{
    public void MockGetSecretValue(string secretName, string secretValue, int statusCode = 200)
    {
        var responseBody = JsonSerializer.Serialize(new
        {
            SecretString = secretValue,
            Name = secretName,
            ARN = $"arn:aws:secretsmanager:us-east-1:123456789012:secret:{secretName}",
            VersionId = Guid.NewGuid().ToString()
        });

        server
            .Given(
                Request
                    .Create()
                    .WithPath("/")
                    .UsingPost()
                    .WithHeader("X-Amz-Target", new RegexMatcher("secretsmanager.*GetSecretValue"))
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "application/x-amz-json-1.1")
                    .WithBody(responseBody)
            );
    }

    public void MockSecretNotFound(string secretName, int statusCode = 404)
    {
        var errorBody = JsonSerializer.Serialize(new
        {
            __type = "ResourceNotFoundException",
            Message = $"Secrets Manager can't find the specified secret."
        });

        server
            .Given(
                Request
                    .Create()
                    .WithPath("/")
                    .UsingPost()
                    .WithHeader("X-Amz-Target", new RegexMatcher("secretsmanager.*GetSecretValue"))
                    .WithBody(new JsonPathMatcher($"$.SecretId[?(@=='{secretName}')]"))
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "application/x-amz-json-1.1")
                    .WithBody(errorBody)
            );
    }
}
