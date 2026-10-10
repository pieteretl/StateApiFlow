using StateFlowApi;
using StateFlowApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICoordinator, Coordinator>();
builder.Services.AddTransient<IReceiveMessage, ReceiveMessage>();
builder.Services.AddTransient<IDifferMessage, DifferMessage>();
builder.Services.AddTransient<ISendMessage, SendMessage>();
builder.Services.AddTransient<IIsolateMessage, IsolateMessage>();
builder.Services.AddTransient<ICancelMessage, CancelMessage>();
builder.Services.AddTransient<IFailMessage, FailMessage>();
builder.Services.AddTransient<IRetryMessage, RetryMessage>();
builder.Services.AddTransient<IRejectMessage, RejectMessage>();
builder.Services.AddTransient<IStartMessage, StartMessage>();

builder.Services.AddSingleton<ICircuitStateContext, CircuitStateContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();