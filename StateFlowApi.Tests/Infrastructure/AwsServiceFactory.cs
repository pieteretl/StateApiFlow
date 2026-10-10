using StateFlowApi.Tests.Infrastructure.AwsMocks;
using WireMock.Server;

namespace StateFlowApi.Tests.Infrastructure;

public class AwsServiceFactory(WireMockServer server)
{
    private readonly S3MockSetup _s3Setup = new(server);
    private readonly DynamoDbMockSetup _dynamoDbSetup = new(server);
    private readonly SecretsManagerMockSetup _secretsManagerSetup = new(server);

    public S3MockSetup S3 => _s3Setup;
    public DynamoDbMockSetup DynamoDb => _dynamoDbSetup;
    public SecretsManagerMockSetup SecretsManager => _secretsManagerSetup;
    public string BaseUrl => server.Urls[0];

    public void Reset()
    {
        server.Reset();
    }

    public void Dispose()
    {
        server?.Dispose();
    }
}
