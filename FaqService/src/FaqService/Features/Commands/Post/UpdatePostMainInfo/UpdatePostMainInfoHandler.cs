using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.UpdatePostMainInfo;

public class UpdatePostMainInfoHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<UpdatePostMainInfoHandler> _logger;

    public UpdatePostMainInfoHandler(PostsRepository repository,
        ILogger<UpdatePostMainInfoHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(UpdatePostMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var postResult = await _repository.GetById(command.Id, cancellationToken);

        if (postResult.IsFailure)
            return postResult.Error;

        var result = postResult.Value.UpdateMainInfo(command.Title, command.Description);
        if (result.IsFailure)
            return result.Error;

        await _repository.Save(cancellationToken);
        _logger.LogInformation("Updated main info post {PostId}.", postResult.Value);

        return postResult.Value.Id;
    }
}