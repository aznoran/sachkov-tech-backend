using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Domain.Issue;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.IntegrationTests.Modules;

internal static class Seeding
{
    internal record AddModulesWithIssuesResult(Guid ModuleId, Guid IssueId);
    
    internal static async Task<Guid> AddModuleToDatabase(
        IssuesWriteDbContext dbContext,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("title").Value,
            Description.Create("description").Value);

        await dbContext.Modules.AddAsync(module, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return module.Id;
    }
    
    internal static async Task<AddModulesWithIssuesResult> AddModuleWithIssuesToDatabase(
        IssuesWriteDbContext dbContext,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("title").Value,
            Description.Create("description").Value);

        var issue1 = CreateIssue(module.Id);
        var issue2 = CreateIssue(module.Id);

        await dbContext.Modules.AddAsync(module, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        await dbContext.Issues.AddRangeAsync(issue1, issue2);
        
        module.AddIssue(issue1.Id);
        module.AddIssue(issue2.Id);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddModulesWithIssuesResult(module.Id, issue1.Id);
    }

    private static Issue CreateIssue(Guid moduleId) =>
        new (
            IssueId.NewIssueId(),
            Title.Create("title").Value,
            Description.Create("description").Value,
            Guid.NewGuid(),
            moduleId,
            Experience.Create(3).Value);
}