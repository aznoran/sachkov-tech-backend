using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.IssueSolving.Contracts;
using SachkovTech.IssuesReviews.Application.Commands.SendForRevision;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Application.Commands.Approve;

public class ApproveIssueReviewHandler : ICommandHandler<Guid, ApproveIssueReviewCommand>
{
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ApproveIssueReviewCommand> _validator;
    private readonly ILogger<ApproveIssueReviewHandler> _logger;
    private readonly IIssueSolvingContract _contract;

    public ApproveIssueReviewHandler(
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(Modules.IssuesReviews)]
        IUnitOfWork unitOfWork,
        IValidator<ApproveIssueReviewCommand> validator,
        ILogger<ApproveIssueReviewHandler> logger,
        IIssueSolvingContract contract)
    {
        _issueReviewRepository = issueReviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _contract = contract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ApproveIssueReviewCommand command,
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
            return issueReviewResult.Error.ToErrorList();
        
        issueReviewResult.Value.Approve();

        var userIssueId = issueReviewResult.Value.UserIssueId;

        if (userIssueId is null)
        {
            return Errors.General.ValueIsInvalid("user_issue_id").ToErrorList();
        }
        
        var sendIssueForRevisionContractRes = await _contract
            .Approve(userIssueId,cancellationToken);

        if (sendIssueForRevisionContractRes.IsFailure)
        {
            return sendIssueForRevisionContractRes.Error;
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "IssueReview {issueReviewId} is approved",
            issueReviewResult.Value.Id.Value);

        return issueReviewResult.Value.Id.Value;
    }
}