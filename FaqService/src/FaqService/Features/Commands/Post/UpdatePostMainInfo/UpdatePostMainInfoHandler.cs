using CSharpFunctionalExtensions;
using FaqService.Extensions;
using FaqService.Infrastructure;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.UpdatePostMainInfo;

public class UpdatePostMainInfoHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<UpdatePostMainInfoHandler> _logger;
    private readonly SearchRepository _searchRepository;
    private readonly UnitOfWork _unitOfWork;
    private readonly ElasticIndexRecoveryService _indexRecoveryService;

    public UpdatePostMainInfoHandler(PostsRepository repository,
        ILogger<UpdatePostMainInfoHandler> logger,
        SearchRepository searchRepository,
        UnitOfWork unitOfWork,
        ElasticIndexRecoveryService indexRecoveryService)
    {
        _repository = repository;
        _logger = logger;
        _searchRepository = searchRepository;
        _unitOfWork = unitOfWork;
        _indexRecoveryService = indexRecoveryService;
    }

    public async Task<Result<Guid, Error>> Handle(UpdatePostMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        bool indexResult = false;
        try
        {
            var postResult = await _repository.GetById(command.Id, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            var result = postResult.Value.UpdateMainInfo(command.Title, command.Description);
            if (result.IsFailure)
                return result.Error;

            await _repository.Save(cancellationToken);
            
            indexResult = await _searchRepository.IndexPost(postResult.Value);
            
            transaction.Commit();
            
            _logger.LogInformation("Updated main info post {PostId}.", postResult.Value);

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