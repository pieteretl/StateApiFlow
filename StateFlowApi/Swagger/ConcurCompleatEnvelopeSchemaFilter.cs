using Microsoft.OpenApi.Models;
using StateFlowApi.Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StateFlowApi.Swagger;

public class ConcurCompleatEnvelopeSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(MessageEnvelope<ConcurCompleatMessage>))
        {
            return;
        }

        schema.Example = ConcurCompleatSwaggerExampleFactory.CreateEnvelopeExample();
    }
}
