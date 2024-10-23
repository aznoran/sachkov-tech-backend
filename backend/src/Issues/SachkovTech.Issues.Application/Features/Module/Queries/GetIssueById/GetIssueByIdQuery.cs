using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Queries.GetIssueById;

public record GetIssueByIdQuery(Guid IssueId) : IQuery;