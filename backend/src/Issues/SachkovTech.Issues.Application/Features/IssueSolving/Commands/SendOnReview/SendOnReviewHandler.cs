using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssuesReviews;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.IssueSolving.Commands.SendOnReview;

public class SendOnReviewHandler : ICommandHandler<SendOnReviewCommand>
{
    private readonly IUserIssueRepository _userIssueRepository;
    private readonly ILogger<SendOnReviewHandler> _logger;
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SendOnReviewCommand> _validator;

    public SendOnReviewHandler(
        IUserIssueRepository userIssueRepository,
        ILogger<SendOnReviewHandler> logger,
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(SharedKernel.Issues.Issues)] IUnitOfWork unitOfWork,
        IValidator<SendOnReviewCommand> validator)
    {
        _issueReviewRepository = issueReviewRepository;
        _logger = logger;
        _userIssueRepository = userIssueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(SendOnReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var userIssue = await _userIssueRepository
            .GetUserIssueById(UserIssueId.Create(command.UserIssueId), cancellationToken);

        if (userIssue.IsFailure)
        {
            _logger.LogError("UserIssue with {Id} not found", command.UserIssueId);
            return userIssue.Error.ToErrorList();
        }

        var pullRequestUrl = PullRequestUrl.Create(command.PullRequestUrl).Value;

        var sendOnReviewRes = userIssue.Value.SendOnReview(pullRequestUrl);

        if (sendOnReviewRes.IsFailure)
        {
            return sendOnReviewRes.Error.ToErrorList();
        }

        var createIssueReviewRes = await CreateIssueReview(
            command.UserIssueId, command.UserId, pullRequestUrl.Value, cancellationToken);

        if (createIssueReviewRes.IsFailure)
        {
            return createIssueReviewRes.Error;
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Issue with UserIssueId {UserIssueId} was created", command.UserIssueId);
        _logger.LogInformation("IssueReview {issueReviewId} was created", createIssueReviewRes.Value);

        return UnitResult.Success<ErrorList>();
    }
    private async Task<Result<Guid, ErrorList>> CreateIssueReview(
        Guid userIssueId,
        Guid userId,
        string pullRequestUrl,
        CancellationToken cancellationToken)
    {
        var issueReview = IssueReview.Create(
        UserIssueId.Create(userIssueId),
           UserId.Create(userId),
           PullRequestUrl.Create(pullRequestUrl).Value);

        if (issueReview.IsFailure)
        {
            return issueReview.Error.ToErrorList();
        }

        await _issueReviewRepository.Add(issueReview.Value, cancellationToken);

        return issueReview.Value.Id.Value;
    }
}