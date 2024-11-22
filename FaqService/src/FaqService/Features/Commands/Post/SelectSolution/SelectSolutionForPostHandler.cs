using CSharpFunctionalExtensions;
using FaqService.Infrastructure;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.SelectSolution;

public class SelectSolutionForPostHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<SelectSolutionForPostHandler> _logger;
    private readonly SearchRepository _searchRepository;
    private readonly UnitOfWork _unitOfWork;

    public SelectSolutionForPostHandler(PostsRepository repository,
        ILogger<SelectSolutionForPostHandler> logger,
        SearchRepository searchRepository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _searchRepository = searchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(PostSelectSolutionCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var postResult = await _repository.GetById(command.PostId, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            var result = postResult.Value.SelectSolution(command.AnswerId);
            if (result.IsFailure)
                return result.Error;

            await _repository.Save(cancellationToken);
            await _searchRepository.IndexPost(postResult.Value);
            
            transaction.Commit();
            
            _logger.LogInformation("For post {PostId} selected solution {AnswerId}.", postResult.Value, command.AnswerId);

            return postResult.Value.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot update post in transaction");

            transaction.Rollback();
            return 
                Error.Failure("Cannot update post in transaction", "post.update.failure");
        }

    }
}