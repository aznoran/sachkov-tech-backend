using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonsWithPagination;

public record GetLessonsWithPaginationValidatorQuery(int Page, int PageSize) : IQuery;
