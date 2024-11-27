using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.IssueSolving.Enums;
using SachkovTech.Issues.Domain.IssueSolving.Events;
using SachkovTech.Issues.Domain.IssueSolving.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.IssueSolving.Entities;

public class UserIssue : DomainEntity<UserIssueId>
{
    //ef core
    private UserIssue(UserIssueId id) : base(id)
    {

    }
    public UserIssue(
        UserIssueId id,
        Guid userId,
        IssueId issueId,
        ModuleId moduleId) : base(id)
    {
        UserId = userId;
        IssueId = issueId;
        ModuleId = moduleId;

        TakeOnWork();
    }

    public Guid UserId { get; private set; }

    public IssueId IssueId { get; private set; }

    public ModuleId ModuleId { get; private set; }

    public IssueStatus Status { get; private set; }

    public DateTime StartDateOfExecution { get; private set; }

    public DateTime EndDateOfExecution { get; private set; }

    public Attempts Attempts { get; private set; } = null!;

    public PullRequestUrl PullRequestUrl { get; private set; } = PullRequestUrl.Empty;

    private void TakeOnWork()
    {
        StartDateOfExecution = DateTime.UtcNow;
        Status = IssueStatus.AtWork;
        Attempts = Attempts.Create();
    }

    public UnitResult<Error> SendOnReview(PullRequestUrl pullRequestUrl)
    {
        if (Status != IssueStatus.AtWork)
            return Error.Failure("issue.status.invalid", "issue not at work");

        Status = IssueStatus.UnderReview;
        PullRequestUrl = pullRequestUrl;

        var @event = new UserIssueSentOnReviewEvent(Id, UserId, pullRequestUrl);
        AddDomainEvent(@event);

        return Result.Success<Error>();
    }

    public UnitResult<Error> SendForRevision()
    {
        if (Status != IssueStatus.UnderReview)
            return Error.Failure("issue.status.invalid", "issue status should be not completed or under review");

        Status = IssueStatus.AtWork;
        Attempts = Attempts.Add();

        return Result.Success<Error>();

    }

    public UnitResult<Error> StopWorking()
    {
        if (Status != IssueStatus.AtWork)
            return Error.Failure("issue.status.invalid", "issue status should be at work");

        Status = IssueStatus.NotAtWork;

        return Result.Success<Error>();

    }

    public UnitResult<Error> CompleteIssue()
    {
        if (Status != IssueStatus.UnderReview)
            return Error.Failure("issue.invalid.status", "issue status should be under review");

        EndDateOfExecution = DateTime.UtcNow;
        Status = IssueStatus.Completed;

        return new UnitResult<Error>();
    }
}