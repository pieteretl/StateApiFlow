using Microsoft.OpenApi.Any;

namespace StateFlowApi.Swagger;

public static class ConcurCompleatSwaggerExampleFactory
{
    public static IOpenApiAny CreateEnvelopeExample()
    {
        var correlationId = "4f1db5f7d4ac434685458d0e4f3fd54b";

        return new OpenApiObject
        {
            ["id"] = new OpenApiString("message-1"),
            ["message"] = new OpenApiObject
            {
                ["eventData"] = new OpenApiObject
                {
                    ["itineraryId"] = new OpenApiString("itinerary-d78a4f76f8314e4db24b0188e2f524bf"),
                    ["messageIdentity"] = new OpenApiObject
                    {
                        ["operationType"] = new OpenApiString("Created"),
                        ["resource"] = new OpenApiString("Itinerary"),
                        ["resourceId"] = new OpenApiString("resource-efac7d5bfe57491f9d3ce8f9b0fbded2")
                    },
                    ["messageDependency"] = new OpenApiObject
                    {
                        ["messageId"] = new OpenApiString("dependency-92cf9f35a1834867b6ed1f31f72f3f83"),
                        ["operationType"] = new OpenApiString("Queued"),
                        ["resource"] = new OpenApiString("Booking"),
                        ["resourceId"] = new OpenApiString("booking-3086a5f2c8984ab98f3bd8d828ef2110")
                    }
                },
                ["metaData"] = new OpenApiObject
                {
                    ["eventId"] = new OpenApiString("event-a804f1881ee541fd87cc68fd4df14894"),
                    ["messageId"] = new OpenApiString("message-dd6fbb6ee0c44e2bb7a91ff108dcf6db"),
                    ["eventType"] = new OpenApiString("ConcurCompleatEvent"),
                    ["eventSubType"] = new OpenApiString("Success"),
                    ["version"] = new OpenApiString("1.0"),
                    ["entryPointId"] = new OpenApiString("entry-point-b56f6d36f18045b8a01a4ad17435e4f6"),
                    ["managedGroupId"] = new OpenApiString("group-bf6a3ce95d5d4d82acd74286e2d75c36"),
                    ["corporateId"] = new OpenApiString("corporate-16195f56afef4df9a27f663e2fcaef89"),
                    ["tenant"] = new OpenApiString("test-tenant"),
                    ["contextUri"] = new OpenApiString($"https://example.test/contexts/{correlationId}"),
                    ["organisationalUnitCode"] = new OpenApiString("ou-3741"),
                    ["integratorId"] = new OpenApiString("integrator-f281dc0f6af7422db5b58db30e4ceb36"),
                    ["timeUtc"] = new OpenApiString("2026-01-15T08:30:00Z"),
                    ["conversationId"] = new OpenApiString(correlationId),
                    ["deliveryStatusCallbackUri"] = new OpenApiString($"https://example.test/callbacks/{correlationId}"),
                    ["s3ObjectKey"] = new OpenApiString($"messages/2026/01/15/{correlationId}.json")
                }
            },
            ["messageTypeIdentifier"] = new OpenApiString("ConcurCompleatMessage"),
            ["sqsMetadata"] = new OpenApiObject
            {
                ["messageID"] = new OpenApiString("message-1"),
                ["receiptHandle"] = new OpenApiString("receipt-handle"),
                ["approximateReceiveCount"] = new OpenApiInteger(1)
            }
        };
    }
}

