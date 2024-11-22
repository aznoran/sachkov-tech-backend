using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.Issues.IntegrationTests.Lessons.AddLessonTests;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.IntegrationTests.Lessons;

public class LessonsTestsBase : IClassFixture<AddLessonTestWebAppFactory>, IAsyncLifetime
{
    protected readonly AddLessonTestWebAppFactory Factory;
    protected readonly IssuesWriteDbContext WriteDbContext;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly IServiceScope Scope;
    protected readonly Fixture Fixture;

    private readonly Func<Task> _resetDatabase;

    protected LessonsTestsBase(AddLessonTestWebAppFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;

        Scope = factory.Services.CreateScope();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<IssuesWriteDbContext>();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        Fixture = new Fixture();
        Factory = factory;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _resetDatabase();
        Scope.Dispose();
    }

    protected async Task<Guid> SeedModule()
    {
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("title").Value,
            Description.Create("description").Value);

        await WriteDbContext.Modules.AddAsync(module);

        await WriteDbContext.SaveChangesAsync();

        return module.Id;
    }
}