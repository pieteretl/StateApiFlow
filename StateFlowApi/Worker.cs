using StateFlowApi.Application;
using StateFlowApi.Infrastructure;

namespace StateFlowApi;

public sealed class Worker(ILogger<Worker> logger, SqsClient sqsClient, ICoordinator coordinator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await sqsClient.Poll();
            
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var result = await coordinator.StartAsync(message, stoppingToken);
        }
    }
}