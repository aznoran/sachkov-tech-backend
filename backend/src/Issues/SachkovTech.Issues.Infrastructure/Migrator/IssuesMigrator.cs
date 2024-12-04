using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.Infrastructure.Migrator;

public class IssuesMigrator(IssuesWriteDbContext context, ILogger<IssuesMigrator> logger) : IMigrator
{
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        logger.Log(LogLevel.Information, "Applying issues migrations...");
        
        if (await context.Database.CanConnectAsync(cancellationToken) == false)
        {
            throw new Exception($"Can't connect to database");
        }

        var createResult = await context.Database.EnsureCreatedAsync(cancellationToken);
        
        if(createResult is false)
            await context.Database.MigrateAsync(cancellationToken);
        
        logger.Log(LogLevel.Information, "Migrations issues applied successfully.");
    }
}