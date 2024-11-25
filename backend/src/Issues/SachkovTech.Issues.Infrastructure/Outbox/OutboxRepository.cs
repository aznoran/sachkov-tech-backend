using System.Text.Json;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Infrastructure.Outbox;

public class OutboxRepository : IOutboxRepository
{
    private readonly IssuesWriteDbContext _dbContext;
    public OutboxRepository(IssuesWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync<TId>(DomainEntity<TId> entity, CancellationToken cancellationToken) where TId : IComparable<TId>
    {
        var outboxMessages = entity.DomainEvents.Select(domainEvent => new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.Now,
            Type = domainEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(domainEvent)
        });

        await _dbContext.AddRangeAsync(outboxMessages, cancellationToken);

        entity.ClearDomainEvents();
    }
}