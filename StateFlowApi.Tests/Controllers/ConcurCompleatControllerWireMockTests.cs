using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StateFlowApi;
using StateFlowApi.Contracts;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace StateFlowApi.Tests.Controllers;

public class ConcurCompleatControllerWireMockTests : IAsyncLifetime
{
    private readonly WireMockServer _wireMockServer = WireMockServer.Start();
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;

    public Task InitializeAsync()
    {
        ConfigureWireMockServer();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<ISendMessage>();
                    services.AddSingleton<ISendMessage>(_ =>
                        new WireMockBackedSendMessage(new HttpClient { BaseAddress = new Uri(_wireMockServer.Urls[0]) }));
                });
            });

        _client = _factory.CreateClient();

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
        _wireMockServer.Stop();
        _wireMockServer.Dispose();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task PostOrder_WithEnvelopeBody_ReturnsSuccess_AndCallsWireMockDependencies()
    {
        var requestBody = CreateEnvelope();

        var response = await _client!.PostAsJsonAsync("/api/concurcompleat", requestBody);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<bool>();
        Assert.True(result);

        var s3Calls = _wireMockServer.FindLogEntries(Request.Create().WithPath("/s3/orders/*").UsingGet());
        var dynamoDbCalls = _wireMockServer.FindLogEntries(Request.Create().WithPath("/dynamodb/orders").UsingPost());
        var secretsManagerCalls = _wireMockServer.FindLogEntries(Request.Create().WithPath("/secrets/orders").UsingPost());

        Assert.NotEmpty(s3Calls);
        Assert.NotEmpty(dynamoDbCalls);
        Assert.NotEmpty(secretsManagerCalls);
    }

    [Fact]
    public async Task SwaggerJson_ShouldContainConcurCompleatEnvelopeExample()
    {
        var swaggerResponse = await _client!.GetStringAsync("/swagger/v1/swagger.json");

        Assert.Contains("ConcurCompleatMessage", swaggerResponse);
        Assert.Contains("messages/2026/01/15", swaggerResponse);
        Assert.Contains("messageTypeIdentifier", swaggerResponse);
    }

    private static MessageEnvelope<ConcurCompleatMessage> CreateEnvelope()
    {
        var correlationId = Guid.NewGuid().ToString("N");
        var now = DateTime.UtcNow;

        return new MessageEnvelope<ConcurCompleatMessage>
        {
            Id = "message-1",
            Message = new ConcurCompleatMessage
            {
                EventData = new EventData
                {
                    ItineraryId = $"itinerary-{Guid.NewGuid():N}",
                    MessageIdentity = new MessageIdentity
                    {
                        OperationType = "Created",
                        Resource = "Itinerary",
                        ResourceId = $"resource-{Guid.NewGuid():N}"
                    },
                    MessageDependency = new MessageDependency
                    {
                        MessageId = $"dependency-{Guid.NewGuid():N}",
                        OperationType = "Queued",
                        Resource = "Booking",
                        ResourceId = $"booking-{Guid.NewGuid():N}"
                    }
                },
                MetaData = new MetaData
                {
                    EventId = $"event-{Guid.NewGuid():N}",
                    MessageId = $"message-{Guid.NewGuid():N}",
                    EventType = "ConcurCompleatEvent",
                    EventSubType = "Success",
                    Version = "1.0",
                    EntryPointId = $"entry-point-{Guid.NewGuid():N}",
                    ManagedGroupId = $"group-{Guid.NewGuid():N}",
                    CorporateId = $"corporate-{Guid.NewGuid():N}",
                    Tenant = "test-tenant",
                    ContextUri = $"https://example.test/contexts/{correlationId}",
                    OrganisationalUnitCode = "ou-1234",
                    IntegratorId = $"integrator-{Guid.NewGuid():N}",
                    TimeUtc = now,
                    ConversationId = correlationId,
                    DeliveryStatusCallbackUri = $"https://example.test/callbacks/{correlationId}",
                    S3ObjectKey = $"messages/{now:yyyy/MM/dd}/{correlationId}.json"
                }
            },
            MessageTypeIdentifier = nameof(ConcurCompleatMessage),
            SQSMetadata = new SQSMetadata
            {
                MessageID = "message-1",
                ReceiptHandle = "receipt-handle",
                ApproximateReceiveCount = 1
            }
        };
    }

    private void ConfigureWireMockServer()
    {
        _wireMockServer
            .Given(Request.Create().WithPath("/s3/orders/*").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBody("payload"));

        _wireMockServer
            .Given(Request.Create().WithPath("/dynamodb/orders").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBody("{}"));

        _wireMockServer
            .Given(Request.Create().WithPath("/secrets/orders").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBody("{\"SecretString\":\"test\"}"));
    }

    private sealed class WireMockBackedSendMessage(HttpClient httpClient) : ISendMessage
    {
        public async Task<MessageState> Process(ConcurCompleatMessage message)
        {
            var messageId = ResolveMessageId(message);

            var s3Response = await httpClient.GetAsync($"/s3/orders/{messageId}");
            var dynamoDbResponse = await httpClient.PostAsJsonAsync("/dynamodb/orders", new { MessageId = messageId });
            var secretsResponse = await httpClient.PostAsJsonAsync("/secrets/orders", new { SecretId = "external-api-key" });

            if (s3Response.IsSuccessStatusCode && dynamoDbResponse.IsSuccessStatusCode && secretsResponse.IsSuccessStatusCode)
            {
                return MessageState.Finish;
            }

            return MessageState.Fail;
        }

        private static string ResolveMessageId(ConcurCompleatMessage message)
        {
            if (!string.IsNullOrWhiteSpace(message.MetaData.ConversationId))
            {
                return message.MetaData.ConversationId;
            }

            if (!string.IsNullOrWhiteSpace(message.MetaData.MessageId))
            {
                return message.MetaData.MessageId;
            }

            return Guid.NewGuid().ToString("N");
        }
    }
}
