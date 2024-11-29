using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Answer.CreateAnswer;

public class CreateAnswerHandler
{
    private readonly ILogger<CreateAnswerHandler> _logger;
    private readonly PostsRepository _repository;

    public CreateAnswerHandler(
        ILogger<CreateAnswerHandler> logger,
        PostsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> Handle(CreateAnswerCommand command, CancellationToken cancellationToken)
    {
        var post = await _repository.GetById(command.PostId, cancellationToken);
        if (post.IsFailure)
            return post.Error;

        var answerResult = Entities.Answer.Create(
            Guid.Empty,
            command.PostId,
            command.Text,
            command.UserId);

        if (answerResult.IsFailure)
            return answerResult.Error;

        var result = post.Value.AddAnswer(answerResult.Value);
        if (result.IsFailure)
            return result.Error;

        await _repository.Save(cancellationToken);

        _logger.LogInformation($"Created answer with id: {command.PostId}");

        return answerResult.Value.Id;
    }
}