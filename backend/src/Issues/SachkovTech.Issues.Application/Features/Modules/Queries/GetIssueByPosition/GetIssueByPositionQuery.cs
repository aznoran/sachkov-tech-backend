using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.Modules.Queries.GetIssueByPosition;

public record GetIssueByPositionQuery(ModuleId ModuleId, int Position) : IQuery;