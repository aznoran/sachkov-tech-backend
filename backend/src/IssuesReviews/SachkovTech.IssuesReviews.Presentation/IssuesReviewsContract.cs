using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.IssuesReviews.Application.Commands.Create;
using SachkovTech.IssuesReviews.Application.Commands.SendForRevision;
using SachkovTech.IssuesReviews.Contracts;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Presentation;

public class IssuesReviewsContract(
    AddCommentHandler addCommentHandler,
    CreateIssueReviewHandler createIssueReviewHandler,
    SendForRevisionHandler sendForRevisionHandler) : IIssuesReviewsContract
{
    public async Task<Result<Guid,ErrorList>> AddComment(
        Guid issueReviewId,
        Guid userId,
        AddCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        return await addCommentHandler.Handle(
            new AddCommentCommand(issueReviewId, userId, request.Message),
            cancellationToken);
    }

    public async Task<Result<Guid,ErrorList>> CreateIssueReview(Guid userIssueId, Guid userId, CreateIssueReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateIssueReviewCommand(userIssueId, userId, request.PullRequestUrl);

        return await createIssueReviewHandler.Handle(command, cancellationToken);
    }

    public async Task<Result<Guid,ErrorList>> SendIssueReviewForRevision(Guid userIssueId, CancellationToken cancellationToken = default)
    {
        var command = new SendForRevisionCommand(userIssueId);

        return await sendForRevisionHandler.Handle(command, cancellationToken);
    }
}