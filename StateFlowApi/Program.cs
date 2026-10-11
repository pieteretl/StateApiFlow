using StateFlowApi;
using StateFlowApi.Application;
using StateFlowApi.Controllers;
using StateFlowApi.Infrastructure;
using StateFlowApi.OtherStates;
using StateFlowApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<ConcurCompleatEnvelopeSchemaFilter>();
    options.OperationFilter<OrderRequestExampleOperationFilter>();
});
builder.Services.AddTransient<ICoordinator, Coordinator>();
builder.Services.AddTransient<IReceiveMessage, ReceiveMessage>();
builder.Services.AddTransient<IDeferMessage, DeferMessage>();
builder.Services.AddTransient<ISendMessage, SendMessage>();
builder.Services.AddTransient<IIsolateMessage, IsolateMessage>();
builder.Services.AddTransient<IFinishMessage, FinishMessage>();
builder.Services.AddTransient<IFailMessage, FailMessage>();
builder.Services.AddTransient<IRetryMessage, RetryMessage>();
builder.Services.AddTransient<IRejectMessage, RejectMessage>();
builder.Services.AddTransient<IStartMessage, StartMessage>();

builder.Services.AddSingleton<ICircuitStateContext, CircuitStateContext>();

builder.Services.AddTransient<IDynamoDbClient, DynamoDbClient>();
builder.Services.AddTransient<IS3Client, S3Client>();
builder.Services.AddTransient<ISecretClient, SecretClient>();
builder.Services.AddTransient<ITokeniseClient, TokeniserClient>();
builder.Services.AddTransient<ISqsClient, SqsClient>();

// builder.Services.AddHostedService<Worker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}