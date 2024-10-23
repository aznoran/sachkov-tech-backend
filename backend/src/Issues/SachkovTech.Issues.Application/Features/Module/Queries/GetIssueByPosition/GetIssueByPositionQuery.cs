using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Queries.GetIssueByPosition;

public record GetIssueByPositionQuery(int Position) : IQuery;