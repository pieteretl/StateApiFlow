using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace StateFlowApi.Tests.Infrastructure.AwsMocks;

public class S3MockSetup(WireMockServer server)
{
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
}
