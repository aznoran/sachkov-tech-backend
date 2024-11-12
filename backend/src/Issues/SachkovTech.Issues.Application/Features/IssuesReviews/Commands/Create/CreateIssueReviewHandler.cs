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

namespace SachkovTech.Issues.Application.Features.IssuesReviews.Commands.Create;

public class CreateIssueReviewHandler : ICommandHandler<Guid, CreateIssueReviewCommand>
{
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateIssueReviewCommand> _validator;
    private readonly ILogger<CreateIssueReviewHandler> _logger;

    public CreateIssueReviewHandler(
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(SharedKernel.Modules.Issues)] IUnitOfWork unitOfWork,
        IValidator<CreateIssueReviewCommand> validator,
        ILogger<CreateIssueReviewHandler> logger)
    {
        _issueReviewRepository = issueReviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateIssueReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var issueReview = IssueReview.Create(
            UserIssueId.Create(command.UserIssueId),
            UserId.Create(command.UserId),
            PullRequestUrl.Create(command.PullRequestUrl).Value);

        if (issueReview.IsFailure)
        {
            return issueReview.Error.ToErrorList();
        }

        await _issueReviewRepository.Add(issueReview.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("IssueReview {issueReviewId} was created", issueReview.Value.Id);

        return issueReview.Value.Id.Value;
    }
}