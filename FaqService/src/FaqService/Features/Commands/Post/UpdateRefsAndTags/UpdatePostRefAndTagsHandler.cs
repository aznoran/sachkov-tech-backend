using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.UpdateRefsAndTags;

public class UpdatePostRefAndTagsHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<UpdatePostRefAndTagsHandler> _logger;

    public UpdatePostRefAndTagsHandler(PostsRepository repository,
        ILogger<UpdatePostRefAndTagsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(UpdatePostRefAndTagsCommand command,
        CancellationToken cancellationToken)
    {
        var postResult = await _repository.GetById(command.Id, cancellationToken);

        if (postResult.IsFailure)
            return postResult.Error;

        postResult.Value.UpdateRefsAndTags(command.ReplLink, command.IssueId, command.LessonId, command.Tags);

        await _repository.Save(cancellationToken);
        _logger.LogInformation("Updated refs and tags post {PostId}.", postResult.Value);

        return postResult.Value.Id;
    }
}