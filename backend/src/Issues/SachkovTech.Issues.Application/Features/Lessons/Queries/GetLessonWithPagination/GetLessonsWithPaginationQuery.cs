using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;

public record GetLessonsWithPaginationQuery(int Page, int PageSize) : IQuery;
