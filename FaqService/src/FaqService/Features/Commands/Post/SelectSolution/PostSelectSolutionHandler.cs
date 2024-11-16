using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.SelectSolution;

public class PostSelectSolutionHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<PostSelectSolutionHandler> _logger;

    public PostSelectSolutionHandler(PostsRepository repository,
        ILogger<PostSelectSolutionHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(PostSelectSolutionCommand command,
        CancellationToken cancellationToken)
    {
        var postResult = await _repository.GetById(command.PostId, cancellationToken);

        if (postResult.IsFailure)
            return postResult.Error;

        var result = postResult.Value.SelectSolution(command.AnswerId);
        if (result.IsFailure)
            return result.Error;

        await _repository.Save(cancellationToken);
        _logger.LogInformation("For post {PostId} selected solution {AnswerId}.", postResult.Value, command.AnswerId);

        return postResult.Value.Id;
    }
}