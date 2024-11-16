using FaqService.Api.Contracts;
using FaqService.Enums;
using FaqService.Features.Commands.Answer.ChangeRating;
using FaqService.Features.Commands.Answer.CreateAnswer;
using FaqService.Features.Commands.Answer.UpdateMainInfo;
using FaqService.Features.Commands.Post.CreatePost;
using FaqService.Features.Commands.Post.SelectSolution;
using FaqService.Features.Commands.Post.UpdatePostMainInfo;
using FaqService.Features.Commands.Post.UpdateRefsAndTags;
using FaqService.Features.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace FaqService.Api.Controllers;

public class PostController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePostRequest request,
        [FromServices] CreatePostHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<IActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdatePostMainInfoRequest request,
        [FromServices] UpdatePostMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/refs-and-tags")]
    public async Task<IActionResult> UpdateRefsAndTags(
        [FromRoute] Guid id,
        [FromBody] UpdatePostRefAndTagsRequest request,
        [FromServices] UpdatePostRefAndTagsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/select-solution/{answerId:guid}/answer")]
    public async Task<IActionResult> SelectSolution(
        [FromRoute] Guid id,
        [FromRoute] Guid answerId,
        [FromServices] PostSelectSolutionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new PostSelectSolutionCommand(id, answerId);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/answer")]
    public async Task<IActionResult> AddAnswer(
        [FromRoute] Guid id,
        [FromBody] CreateAnswerRequest request,
        [FromServices] CreateAnswerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPut("{postId:guid}/answer/{answerId:guid}/main-info")]
    public async Task<IActionResult> UpdateAnswerMainInfo(
        [FromRoute] Guid postId,
        [FromRoute] Guid answerId,
        [FromBody] UpdateAnswerMainInfoRequest request,
        [FromServices] UpdateAnswerMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(postId, answerId), cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPut("{postId:guid}/answer/{answerId:guid}/increase-rating")]
    public async Task<IActionResult> IncreaseAnswerRating(
        [FromRoute] Guid postId,
        [FromRoute] Guid answerId,
        [FromServices] IncreaseAnswerRatingHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new IncreaseAnswerRatingCommand(postId, answerId);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpPut("{postId:guid}/answer/{answerId:guid}/decrease-rating")]
    public async Task<IActionResult> DecreaseAnswerRating(
        [FromRoute] Guid postId,
        [FromRoute] Guid answerId,
        [FromServices] DecreaseAnswerRatingHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DecreaseAnswerRatingCommand(postId, answerId);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string? searchText,
        [FromQuery] Status? status,
        [FromQuery] bool? sortByDateDescending,
        [FromQuery] List<Guid>? tags,
        [FromQuery] Guid? issueId,
        [FromQuery] Guid? lessonId,
        [FromServices] GetPostsWithPaginationAndFiltersHandler handler,
        CancellationToken cancellationToken = default)
    {
        var paginatedPosts = await handler.Handle(
            pageNumber,
            pageSize,
            searchText,
            status,
            sortByDateDescending,
            tags,
            issueId,
            lessonId,
            cancellationToken);

        return Ok(paginatedPosts);
    }
    
    [HttpGet("{id:guid}/answers")]
    public async Task<IActionResult> GetAllAnswersOfPost(
        [FromRoute] Guid id,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] GetAnswersWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var answers = await handler.Handle(
            id,
            pageNumber,
            pageSize,
            cancellationToken);

        return Ok(answers);
    }
    
    [HttpGet("{id:guid}/answer/{answerId:guid}")]
    public async Task<IActionResult> GetOneAnswerOfPost(
        [FromRoute] Guid id,
        [FromRoute] Guid answerId,
        [FromServices] GetAnswerAtPostByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var answer = await handler.Handle(
            id,
            answerId,
            cancellationToken);

        return Ok(answer);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(
        [FromRoute] Guid id,
        [FromServices] DeletePostHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePostCommand(id);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        
        return Ok(result.Value);
    }
    
    [HttpDelete("{postId:guid}/answer/{answerId:guid}")]
    public async Task<IActionResult> DeletePost(
        [FromRoute] Guid postId,
        [FromRoute] Guid answerId,
        [FromServices] DeleteAnswerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAnswerCommand(postId, answerId);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        
        return Ok(result.Value);
    }
}