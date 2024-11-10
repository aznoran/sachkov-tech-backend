using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Issue.Queries.GetIssueById;

public record GetIssueByIdQuery(Guid IssueId) : IQuery;