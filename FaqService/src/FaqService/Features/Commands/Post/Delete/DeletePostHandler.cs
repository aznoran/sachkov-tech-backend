using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.Delete;

public class DeletePostHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<DeletePostHandler> _logger;
    private readonly SearchRepository _searchRepository;

    public DeletePostHandler(
        PostsRepository repository,
        ILogger<DeletePostHandler> logger,
        SearchRepository searchRepository)
    {
        _repository = repository;
        _logger = logger;
        _searchRepository = searchRepository;
    }

    public async Task<Result<Guid, Error>> Handle(DeletePostCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _repository.Delete(command.PostId, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        await _repository.Save(cancellationToken);
        await _searchRepository.DeletePost(command.PostId, cancellationToken);
        
        _logger.LogInformation("Post {PostId} was deleted.", command.PostId);

        return command.PostId;
    }
}