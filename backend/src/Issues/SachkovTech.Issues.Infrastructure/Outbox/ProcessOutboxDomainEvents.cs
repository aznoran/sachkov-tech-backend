using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Quartz;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxDomainEvents : IJob
{
    private readonly IssuesWriteDbContext _dbContext;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<ProcessOutboxDomainEvents> _logger;

    public ProcessOutboxDomainEvents(IssuesWriteDbContext dbContext, IPublishEndpoint publisher, ILogger<ProcessOutboxDomainEvents> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .Set<OutboxMessage>()
            .AsNoTracking()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Constant,
                Delay = TimeSpan.Zero,
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not NullReferenceException),
                OnRetry = retryArguments =>
                {
                    _logger.LogCritical(retryArguments.Outcome.Exception, "Current attempt: {attemptNumber}", retryArguments.AttemptNumber);

                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        foreach (var message in messages)
        {
            try
            {
                await pipeline.ExecuteAsync(async token =>
                {
                    throw new Exception();
                    var messageType = IntegrationEvents.AssemblyReference.Assembly.GetType(message.Type)
                                      ?? throw new NullReferenceException("Message type not found");

                    var deserializedMessage = JsonSerializer.Deserialize(message.Payload, messageType)
                                              ?? throw new NullReferenceException("Message payload not found");

                    await _publisher.Publish(deserializedMessage, messageType, token);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync(token);
                });
            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();
                message.ProcessedOnUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}