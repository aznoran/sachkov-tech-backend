using FaqService.Infrastructure.Repositories;

namespace FaqService.Extensions;

public class ElasticIndexRecoveryService
{
    private readonly PostsRepository _repository;
    private readonly SearchRepository _searchRepository;
    private readonly ILogger<ElasticIndexRecoveryService> _logger;

    public ElasticIndexRecoveryService(
        PostsRepository repository,
        SearchRepository searchRepository,
        ILogger<ElasticIndexRecoveryService> logger)
    {
        _repository = repository;
        _searchRepository = searchRepository;
        _logger = logger;
    }

    public async Task RestoreElasticIndex(Guid postId, bool indexResult, CancellationToken cancellationToken)
    {
        if (!indexResult)
            return;

        _logger.LogWarning("Restoring previous index state for post {PostId}.", postId);

        var postResult = await _repository.GetById(postId, cancellationToken);

        if (postResult.IsFailure)
        {
            _logger.LogError("Failed to restore index for post {PostId}. Post not found after rollback.", postId);
            return;
        }

        try
        {
            await _searchRepository.IndexPost(postResult.Value);
            _logger.LogInformation("Successfully restored index for post {PostId}.", postId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore index for post {PostId} after rollback.", postId);
        }
    }
}