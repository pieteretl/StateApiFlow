using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

public class S3MockSetup(WireMockServer server)
{
    public void MockGetJsonObject(string bucketName, string key, string jsonContent, int statusCode = 200)
    {
        server
            .Given(
                Request
                    .Create()
                    .WithPath($"/{bucketName}/{key}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(jsonContent)
            );
    }

    public void MockGetJsonObjectFromFile(string bucketName, string key, string filePath, int statusCode = 200)
    {
        var jsonContent = File.ReadAllText(filePath);
        MockGetJsonObject(bucketName, key, jsonContent, statusCode);
    }

    public void MockGetObject(string bucketName, string key, string content, int statusCode = 200)
    {
        server
            .Given(
                Request
                    .Create()
                    .WithPath($"/{bucketName}/{key}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBody(content)
            );
    }

    public void MockObjectNotFound(string bucketName, string key)
    {
        var body =
            "<Error><Code>NoSuchKey</Code><Message>The specified key does not exist.</Message><Key>" +
            key +
            "</Key><BucketName>" +
            bucketName +
            "</BucketName></Error>";

        server
            .Given(
                Request
                    .Create()
                    .WithPath($"/{bucketName}/{key}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(404)
                    .WithHeader("Content-Type", "application/xml")
                    .WithBody(body)
            );
    }
}
