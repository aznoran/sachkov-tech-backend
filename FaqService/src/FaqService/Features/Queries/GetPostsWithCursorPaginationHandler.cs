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
    private readonly SearchRepository _searchRepository;

    public GetPostsWithCursorPaginationHandler(
        PostsRepository repository,
        SearchRepository searchRepository)
    {
        _repository = repository;
        _searchRepository = searchRepository;
    }

    public async Task<CursorList<PostDto>> Handle(
        GetPostsQuery query,
        CancellationToken cancellationToken = default)
    {
        var postIds = await _searchRepository.SearchPosts(query, cancellationToken);
        
        var cursorIndex = query.Cursor.HasValue ? postIds.IndexOf(query.Cursor.Value) : -1;
        
        if (query.Cursor.HasValue && cursorIndex == -1)
        {
            return new CursorList<PostDto>(
                items: new List<PostDto>(),
                cursor: query.Cursor,
                nextCursor: null,
                limit: query.Limit
            );
        }
        
        var paginatedPostIds = cursorIndex >= 0
            ? postIds.Skip(cursorIndex + 1).Take(query.Limit).ToList()
            : postIds.Take(query.Limit).ToList();
        
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
        if (postIds.Count > cursorIndex + 1 + query.Limit)
        {
            var nextCursorIndex = cursorIndex + query.Limit + 1;
            nextCursor = postIds[nextCursorIndex];
        }

        return new CursorList<PostDto>(
            items: postDtos,
            cursor: query.Cursor,
            nextCursor: nextCursor,
            limit: query.Limit,
            totalCount: postIds.Count
        );
    }
}