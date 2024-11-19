using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.IntegrationTests.Lessons;

public abstract class LessonsTestsBase : IClassFixture<IntegrationTestsWebAppFactory>, IAsyncDisposable
{
    protected readonly ILessonsRepository Repository;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IssuesWriteDbContext WriteDbContext;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly IServiceScope Scope;

    protected LessonsTestsBase(IntegrationTestsWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();

        Repository = Scope.ServiceProvider.GetRequiredService<ILessonsRepository>();
        UnitOfWork = Scope.ServiceProvider.GetRequiredKeyedService<IUnitOfWork>(Modules.Issues);
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