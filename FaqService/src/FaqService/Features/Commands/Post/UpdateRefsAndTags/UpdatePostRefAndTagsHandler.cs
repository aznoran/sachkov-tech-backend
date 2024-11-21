using CSharpFunctionalExtensions;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.UpdateRefsAndTags;

public class UpdatePostRefAndTagsHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<UpdatePostRefAndTagsHandler> _logger;
    private readonly SearchRepository _searchRepository;

    public UpdatePostRefAndTagsHandler(PostsRepository repository,
        ILogger<UpdatePostRefAndTagsHandler> logger,
        SearchRepository searchRepository)
    {
        _repository = repository;
        _logger = logger;
        _searchRepository = searchRepository;
    }

    public async Task<Result<Guid, Error>> Handle(UpdatePostRefAndTagsCommand command,
        CancellationToken cancellationToken)
    {
        var postResult = await _repository.GetById(command.Id, cancellationToken);

        if (postResult.IsFailure)
            return postResult.Error;

        postResult.Value.UpdateRefsAndTags(command.ReplLink, command.IssueId, command.LessonId, command.Tags);

        await _repository.Save(cancellationToken);
        await _searchRepository.IndexPost(postResult.Value);
        _logger.LogInformation("Updated refs and tags post {PostId}.", postResult.Value);

        return postResult.Value.Id;
    }
}