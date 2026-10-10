using Microsoft.OpenApi.Models;
using StateFlowApi.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StateFlowApi.Swagger;

public class OrderRequestExampleOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType != typeof(ConcurCompleatController) || context.MethodInfo.Name != nameof(ConcurCompleatController.Create))
        {
            return;
        }

        if (operation.RequestBody?.Content.TryGetValue("application/json", out var mediaType) == true)
        {
            mediaType.Example = ConcurCompleatSwaggerExampleFactory.CreateEnvelopeExample();
        }
    }
}

