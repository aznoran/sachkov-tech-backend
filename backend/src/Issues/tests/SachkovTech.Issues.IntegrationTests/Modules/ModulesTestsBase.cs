using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public abstract class ModulesTestsBase : IClassFixture<IntegrationTestsWebAppFactory>, IAsyncDisposable
{
    protected readonly IServiceScope Scope;
    protected readonly IModulesRepository Repository;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IssuesWriteDbContext WriteDbContext;
    protected readonly IReadDbContext ReadDbContext;
    
    protected ModulesTestsBase(IntegrationTestsWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();

        Repository = Scope.ServiceProvider.GetRequiredService<IModulesRepository>();
        UnitOfWork = Scope.ServiceProvider.GetRequiredKeyedService<IUnitOfWork>(SharedKernel.Modules.Issues);
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<IssuesWriteDbContext>();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
    }
    
    public async ValueTask DisposeAsync()
    {
        if (Scope is IAsyncDisposable scopeAsyncDisposable)
            await scopeAsyncDisposable.DisposeAsync();
        else
            Scope.Dispose();
    }
}