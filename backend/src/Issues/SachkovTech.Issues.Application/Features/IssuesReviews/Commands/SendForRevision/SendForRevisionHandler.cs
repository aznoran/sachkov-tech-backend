using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.IssuesReviews.Commands.SendForRevision;

public class SendForRevisionHandler : ICommandHandler<Guid, SendForRevisionCommand>
{
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUserIssueRepository _userIssueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SendForRevisionCommand> _validator;
    private readonly ILogger<SendForRevisionHandler> _logger;

    public SendForRevisionHandler(
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(Modules.Issues)]
        IUnitOfWork unitOfWork,
        IValidator<SendForRevisionCommand> validator,
        ILogger<SendForRevisionHandler> logger,
        IUserIssueRepository userIssueRepository)
    {
        _issueReviewRepository = issueReviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _userIssueRepository = userIssueRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SendForRevisionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var issueReviewResult = await _issueReviewRepository
            .GetById(IssueReviewId.Create(command.IssueReviewId), cancellationToken);

        if (issueReviewResult.IsFailure)
        {
            return issueReviewResult.Error.ToErrorList();
        }

        issueReviewResult.Value.SendIssueForRevision(UserId.Create(command.ReviewerId));

        var userIssueId = issueReviewResult.Value.UserIssueId;

        if (userIssueId is null)
        {
            return Errors.General.ValueIsInvalid("user_issue_id").ToErrorList();
        }

        var sendIssueForRevisionContractRes = 
            await SendIssueForRevision(userIssueId, cancellationToken);

        if (sendIssueForRevisionContractRes.IsFailure)
        {
            return sendIssueForRevisionContractRes.Error;
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "IssueReview {issueReviewId} is sent for review",
            issueReviewResult.Value.Id.Value);

        return issueReviewResult.Value.Id.Value;
    }

    private async Task<Result<Guid, ErrorList>> SendIssueForRevision(
        Guid userIssueId, CancellationToken cancellationToken)
    {
        var userIssueResult = await _userIssueRepository
            .GetUserIssueById(UserIssueId.Create(userIssueId), cancellationToken);

        if (userIssueResult.IsFailure)
        {
            return userIssueResult.Error.ToErrorList();
        }

        var sendForRevisionResult = userIssueResult.Value.SendForRevision();

        if (sendForRevisionResult.IsFailure)
        {
            return sendForRevisionResult.Error.ToErrorList();
        }

        return userIssueResult.Value.Id.Value;
    }
}