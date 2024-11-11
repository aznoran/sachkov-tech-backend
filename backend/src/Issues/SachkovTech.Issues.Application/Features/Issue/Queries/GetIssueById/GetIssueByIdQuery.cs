using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.Issue.Queries.GetIssueById;

public record GetIssueByIdQuery(IssueId IssueId) : IQuery;