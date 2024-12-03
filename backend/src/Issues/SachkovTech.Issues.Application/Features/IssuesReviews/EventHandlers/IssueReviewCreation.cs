﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssueSolving.DomainEvents;
using SachkovTech.Issues.Domain.IssuesReviews;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.IssuesReviews.EventHandlers;

public class IssueReviewCreation : INotificationHandler<IssueSentOnReviewEvent>
{
    private readonly IIssuesReviewRepository _issuesReviewRepository;
    private readonly ILogger<IssueReviewCreation> _logger;

    public IssueReviewCreation(
        IIssuesReviewRepository issuesReviewRepository,
        ILogger<IssueReviewCreation> logger,
        [FromKeyedServices(SharedKernel.Modules.Issues)] IUnitOfWork unitOfWork)
    {
        _issuesReviewRepository = issuesReviewRepository;
        _logger = logger;
    }

    public async Task Handle(IssueSentOnReviewEvent domainEvent, CancellationToken cancellationToken)
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