using CSharpFunctionalExtensions;
using SachkovTech.IssueSolving.Application.Commands.CompleteIssue;
using SachkovTech.IssueSolving.Application.Commands.SendForRevision;
using SachkovTech.IssueSolving.Contracts;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssueSolving.Presentation;

public class IssueSolvingContract : IIssueSolvingContract
{
    private readonly SendForRevisionHandler _sendForRevisionHandler;
    private readonly CompleteIssueHandler _completeIssueHandler;

    public IssueSolvingContract(SendForRevisionHandler sendForRevisionHandler,
        CompleteIssueHandler completeIssueHandler)
    {
        _sendForRevisionHandler = sendForRevisionHandler;
        _completeIssueHandler = completeIssueHandler;
    }
    public async Task<Result<Guid,ErrorList>> SendIssueForRevision(Guid userIssueId, CancellationToken cancellationToken = default)
    {
        var command = new SendForRevisionCommand(userIssueId);

        return await _sendForRevisionHandler.Handle(command, cancellationToken);
    }

    public async Task<Result<Guid, ErrorList>> Approve(Guid userIssueId, CancellationToken cancellationToken = default)
    {
        var command = new CompleteIssueCommand(userIssueId);

        return await _completeIssueHandler.Handle(command, cancellationToken);
    }
}