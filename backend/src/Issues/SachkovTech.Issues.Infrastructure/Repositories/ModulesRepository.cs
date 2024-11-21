using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Repositories;

public class ModulesRepository : IModulesRepository
{
    private readonly IssuesWriteDbContext _dbContext;

    public ModulesRepository(IssuesWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Module issue, CancellationToken cancellationToken = default)
    {
        await _dbContext.Modules.AddAsync(issue, cancellationToken);
        return issue.Id;
    }

    public Guid Save(Module issue, CancellationToken cancellationToken = default)
    {
        _dbContext.Modules.Attach(issue);
        return issue.Id.Value;
    }

    public Guid Delete(Module issue)
    {
        _dbContext.Modules.Remove(issue);

        return issue.Id;
    }

    public async Task<Result<Module, Error>> GetById(
        ModuleId moduleId, CancellationToken cancellationToken = default)
    {
        var module = await _dbContext.Modules
            .FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);

        if (module is null)
            return Errors.General.NotFound(moduleId);

        return module;
    }

    public async Task<Result<Module, Error>> GetByTitle(
        Title title, CancellationToken cancellationToken = default)
    {
        var module = await _dbContext.Modules
            .FirstOrDefaultAsync(m => m.Title == title, cancellationToken);

        if (module is null)
            return Errors.General.NotFound();

        return module;
    }
}