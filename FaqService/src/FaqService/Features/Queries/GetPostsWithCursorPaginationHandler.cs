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
        
        var postsQuery =  _repository.QueryPostsByIds(postIds, cancellationToken);
        
        var paginatedPosts = await postsQuery.ToCursorListWithOrderedIds(
            cursor: query.Cursor,
            orderedIds: postIds,
            limit: query.Limit,
            cancellationToken: cancellationToken);
        
        var postDtos = paginatedPosts.Items.Select(post => new PostDto
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
        
        return new CursorList<PostDto>(
            items: postDtos,
            cursor: paginatedPosts.Cursor,
            nextCursor: paginatedPosts.NextCursor,
            limit: paginatedPosts.Limit,
            totalCount: paginatedPosts.TotalCount
        );
    }
}
