using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Answer.Delete;

public class DeleteAnswerHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<DeleteAnswerHandler> _logger;

    public DeleteAnswerHandler(PostsRepository repository,
        ILogger<DeleteAnswerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(DeleteAnswerCommand command,
        CancellationToken cancellationToken)
    {
        var post = await _repository.GetById(command.PostId, cancellationToken);
        if (post.IsFailure)
            return post.Error;

        var delResult = post.Value.DeleteAnswer(command.AnswerId);
        if (delResult.IsFailure)
            return delResult.Error;
        
        await _repository.Save(cancellationToken);
        
        _logger.LogInformation("Answer {AnswerId} was deleted.", command.AnswerId);

        return command.AnswerId;
    }
}