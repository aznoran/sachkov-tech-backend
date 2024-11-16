using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.CreatePost;

public class CreatePostHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<CreatePostHandler> _logger;

    public CreatePostHandler(PostsRepository repository,
        ILogger<CreatePostHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var postResult = Entities.Post.Create(
            Guid.NewGuid(),
            command.Title,
            command.Description,
            command.ReplLink,
            command.UserId,
            command.IssueId,
            command.LessonId,
            command.Tags);

        if (postResult.IsFailure)
            return postResult.Error;

        await _repository.Add(postResult.Value, cancellationToken);

        await _repository.Save(cancellationToken);
        _logger.LogInformation("Created post {PostId}.", postResult.Value);

        return postResult.Value.Id;
    }
}