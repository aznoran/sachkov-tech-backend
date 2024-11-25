using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly IssuesWriteDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(IssuesWriteDbContext dbContext, IPublisher publisher, ILogger<ProcessOutboxMessagesJob> logger)
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

        foreach (var outboxMessage in messages)
        {
            try
            {
                var domainEvent = JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Payload);

                if (domainEvent is null)
                    continue;

                await _publisher.Publish(domainEvent, context.CancellationToken);

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                outboxMessage.Error = ex.Message;

                _logger.LogCritical(ex, "Failed to process outbox message {OutboxMessageId}", outboxMessage.Id);
            }

            try
            {
                await _dbContext.SaveChangesAsync(context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception occured while saving changes to database for outbox message");
            }

        }
    }
}