using FaqService.Api.Contracts;
using FaqService.Dtos;
using FaqService.Entities;
using FaqService.Enums;
using FaqService.Extensions;
using FaqService.Infrastructure;
using FaqService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FaqService.Features.Queries;

public class GetPostsWithCursorPaginationHandler
{
    private readonly PostsRepository _repository;

    public GetPostsWithCursorPaginationHandler(PostsRepository repository)
    {
        _repository = repository;
    }

    public async Task<CursorList<PostDto>> Handle(
        List<Guid> postIds, 
        Guid? cursor, 
        int limit,
        CancellationToken cancellationToken = default)
    {
        var cursorIndex = cursor.HasValue ? postIds.IndexOf(cursor.Value) : -1;
        
        if (cursor.HasValue && cursorIndex == -1)
        {
            return new CursorList<PostDto>(
                items: new List<PostDto>(),
                cursor: cursor,
                nextCursor: null,
                limit: limit
            );
        }
        
        var paginatedPostIds = cursorIndex >= 0
            ? postIds.Skip(cursorIndex + 1).Take(limit).ToList()
            : postIds.Take(limit).ToList();
        
        var posts = await _repository.GetPostsByIds(paginatedPostIds, cancellationToken);

        var postDtos = posts.Value.Select(post => new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            ReplLink = post.ReplLink,
            Status = post.Status,
            CreatedAt = post.CreatedAt,
            Tags = post.Tags,
            IssueId = post.IssueId,
            LessonId = post.LessonId,
            AnswerId = post.AnswerId,
            CountOfAnswers = post.Answers.Count,
        }).ToList();

        Guid? nextCursor = null;
        if (postIds.Count > cursorIndex + 1 + limit)
        {
            var nextCursorIndex = cursorIndex + limit + 1;
            nextCursor = postIds[nextCursorIndex];
        }

        return new CursorList<PostDto>(
            items: postDtos,
            cursor: cursor,
            nextCursor: nextCursor,
            limit: limit,
            totalCount: postIds.Count
        );
    }
}