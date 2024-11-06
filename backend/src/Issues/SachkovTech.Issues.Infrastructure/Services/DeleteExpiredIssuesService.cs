using Microsoft.EntityFrameworkCore;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;

namespace SachkovTech.Issues.Infrastructure.Services;

public class DeleteExpiredIssuesService
{
    private readonly IssuesWriteDbContext _issuesWriteDbContext;

    public DeleteExpiredIssuesService(
        IssuesWriteDbContext issuesWriteDbContext)
    {
        _issuesWriteDbContext = issuesWriteDbContext;
    }
    
    public async Task Process(CancellationToken cancellationToken)
    {
        var modules = await GetModulesWithIssuesAsync(cancellationToken);

        foreach (var module in modules)
        {
            module.DeleteExpiredIssues();
        }

        await _issuesWriteDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Module>> GetModulesWithIssuesAsync(CancellationToken cancellationToken)
    {
        return await _issuesWriteDbContext.Modules.Include(m => m.Issues).ToListAsync(cancellationToken);
    }
}