using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssueSolving.Events;
using SachkovTech.Issues.Domain.IssuesReviews;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.EventHandlers.UserIssueSentOnReviewEventHandlers;

public class IssueReviewCreation : INotificationHandler<UserIssueSentOnReviewEvent>
{
    private readonly IIssuesReviewRepository _issuesReviewRepository;
    private readonly ILogger<IssueReviewCreation> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public IssueReviewCreation(
        IIssuesReviewRepository issuesReviewRepository,
        ILogger<IssueReviewCreation> logger,
        [FromKeyedServices(SharedKernel.Modules.Issues)] IUnitOfWork unitOfWork)
    {
        _issuesReviewRepository = issuesReviewRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserIssueSentOnReviewEvent domainEvent, CancellationToken cancellationToken)
    {
        var issueReviewResult = new IssueReview(
            IssueReviewId.NewIssueReviewId(),
            domainEvent.UserIssueId,
            domainEvent.UserId,
            domainEvent.PullRequestUrl);

        await _issuesReviewRepository.Add(issueReviewResult, cancellationToken);

        _logger.LogInformation("IssueReview {IssueReviewId} was created", issueReviewResult.Id);
    }
}