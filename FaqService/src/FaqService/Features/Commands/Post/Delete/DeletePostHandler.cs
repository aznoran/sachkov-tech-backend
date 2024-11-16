using CSharpFunctionalExtensions;
using FaqService.Api.Contracts;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.SelectSolution;

public class DeletePostHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<DeletePostHandler> _logger;

    public DeletePostHandler(PostsRepository repository,
        ILogger<DeletePostHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(DeletePostCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _repository.Delete(command.PostId, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        await _repository.Save(cancellationToken);
        _logger.LogInformation("Post {PostId} was deleted.", command.PostId);

        return command.PostId;
    }
}