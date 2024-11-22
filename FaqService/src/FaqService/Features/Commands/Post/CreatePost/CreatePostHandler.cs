using CSharpFunctionalExtensions;
using FaqService.Infrastructure;
using FaqService.Infrastructure.Repositories;
using SharedKernel;

namespace FaqService.Features.Commands.Post.CreatePost;

public class CreatePostHandler
{
    private readonly PostsRepository _repository;
    private readonly ILogger<CreatePostHandler> _logger;
    private readonly SearchRepository _searchRepository;
    private readonly UnitOfWork _unitOfWork;

    public CreatePostHandler(PostsRepository repository,
        ILogger<CreatePostHandler> logger,
        SearchRepository searchRepository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _searchRepository = searchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var postResult = Entities.Post.Create(
                Guid.NewGuid(),
                command.Title,
                command.Description,
                command.ReplLink,
                command.UserId,
                command.IssueId,
                command.LessonId,
                command.Tags);

            if (postResult.IsFailure)
                return postResult.Error;

            await _repository.Add(postResult.Value, cancellationToken);

            await _repository.Save(cancellationToken);
            await _searchRepository.IndexPost(postResult.Value);
            
            transaction.Commit();
            
            _logger.LogInformation("Created post {PostId}.", postResult.Value);

            return postResult.Value.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot create post in transaction");

            transaction.Rollback();
            return 
                Error.Failure("Cannot create post in transaction", "post.create.failure");
        }
        
    }
}