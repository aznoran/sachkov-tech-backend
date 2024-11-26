using CSharpFunctionalExtensions;
using FaqService.Extensions;
using FaqService.Infrastructure;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.UpdateRefsAndTags;

public class UpdatePostRefAndTagsHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<UpdatePostRefAndTagsHandler> _logger;
    private readonly SearchRepository _searchRepository;
    private readonly UnitOfWork _unitOfWork;
    private readonly ElasticIndexRecoveryService _indexRecoveryService;

    public UpdatePostRefAndTagsHandler(PostsRepository repository,
        ILogger<UpdatePostRefAndTagsHandler> logger,
        SearchRepository searchRepository,
        UnitOfWork unitOfWork,
        ElasticIndexRecoveryService indexRecoveryService)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _indexRecoveryService = indexRecoveryService;
        _searchRepository = searchRepository;
    }

    public async Task<Result<Guid, Error>> Handle(UpdatePostRefAndTagsCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        var indexResult = false;
        try
        {
            var postResult = await _repository.GetById(command.Id, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            postResult.Value.UpdateRefsAndTags(command.ReplLink, command.IssueId, command.LessonId, command.Tags);

            await _repository.Save(cancellationToken);
            indexResult = await _searchRepository.IndexPost(postResult.Value);
            
            transaction.Commit();
            
            _logger.LogInformation("Updated refs and tags post {PostId}.", postResult.Value);

            return postResult.Value.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot update post in transaction");

            transaction.Rollback();
            
            if (indexResult)
                await _searchRepository.DeletePost(command.Id, cancellationToken);
            
            await _indexRecoveryService.RestoreElasticIndex(command.Id, indexResult, cancellationToken);
            
            return 
                Error.Failure("Cannot update post in transaction", "post.update.failure");
        }
    }
}