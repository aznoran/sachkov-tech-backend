using SachkovTech.Core.Dtos;

namespace SachkovTech.Issues.Application.Interfaces;

public interface IReadDbContext
{
    IQueryable<IssueDto> Issues { get; }
    IQueryable<ModuleDto> Modules { get; }
    IQueryable<IssueReviewDto> IssueReviewDtos { get; }
    IQueryable<CommentDto> Comments { get; }
    IQueryable<UserIssueDto> UserIssues { get; }
    IQueryable<LessonDto> Lessons { get; }
}