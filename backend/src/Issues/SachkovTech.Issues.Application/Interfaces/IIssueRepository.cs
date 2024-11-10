using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Issue;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Interfaces;

public interface IIssueRepository
{
    Task<Guid> Add(Issue issue, CancellationToken cancellationToken = default);
    
    Guid Save(Issue issue, CancellationToken cancellationToken = default);
    
    Guid Delete(Issue issue);
    
    Task<Result<Issue, Error>> GetById(IssueId issueId, CancellationToken cancellationToken = default);
    
    Task<Result<Issue, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default);
}