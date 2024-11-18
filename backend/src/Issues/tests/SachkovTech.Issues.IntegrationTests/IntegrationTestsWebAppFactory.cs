using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace SachkovTech.Issues.IntegrationTests;

public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("sachkov_tech")
        .WithUsername("postrges")
        .WithPassword("postgres")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var writeContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IssuesWriteDbContext));

             var readContext = services.SingleOrDefault(s =>
                 s.ServiceType == typeof(IReadDbContext));

            if (writeContext is not null)
                services.Remove(writeContext);

            if (readContext is not null)
                services.Remove(readContext);

            services.AddScoped<IssuesWriteDbContext>(provider =>
                new IssuesWriteDbContext(_dbContainer.GetConnectionString()));
            
            services.AddScoped<IReadDbContext, IssuesReadDbContext>(provider =>
                new IssuesReadDbContext(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IssuesWriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}