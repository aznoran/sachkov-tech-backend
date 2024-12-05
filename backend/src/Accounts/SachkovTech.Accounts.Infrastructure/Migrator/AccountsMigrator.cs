﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Infrastructure.DbContexts;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Infrastructure.Migrator;

public class AccountsMigrator(AccountsWriteDbContext context, ILogger<AccountsMigrator> logger) : IMigrator
{
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        logger.Log(LogLevel.Information, "Applying accounts migrations...");
        
        if (await context.Database.CanConnectAsync(cancellationToken) is false)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }
        
        await context.Database.MigrateAsync(cancellationToken);
        
        logger.Log(LogLevel.Information, "Migrations accounts applied successfully.");
    }
}
