using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.IntegrationTests.Lessons;

public abstract class LessonsTestsBase : IClassFixture<IntegrationTestsWebAppFactory>, IDisposable
{
    protected readonly ILessonsRepository _repository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IssuesWriteDbContext _writeDbContext;
    protected readonly IReadDbContext _readDbContext;
    protected readonly IServiceScope _scope;

    protected LessonsTestsBase(IntegrationTestsWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        _repository = _scope.ServiceProvider.GetRequiredService<ILessonsRepository>();
        _unitOfWork = _scope.ServiceProvider.GetRequiredKeyedService<IUnitOfWork>(Modules.Issues);
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<IssuesWriteDbContext>();
        _readDbContext = _scope.ServiceProvider.GetRequiredService<IReadDbContext>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}