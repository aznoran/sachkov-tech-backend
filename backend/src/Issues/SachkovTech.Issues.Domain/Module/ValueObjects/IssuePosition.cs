using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Module.ValueObjects;

public class IssuePosition : ValueObject
{
    public IssuePosition(IssueId issueId, Position position)
    {
        IssueId = issueId;
        Position = position;
    }

    public IssueId IssueId { get; }

    public Position Position { get; }

    public IssuePosition Move(Position newPosition) => new IssuePosition(IssueId, newPosition);

    public Result<IssuePosition, Error> MoveForward()
    {
        var issuePositionResult = Position.Forward();
        if (issuePositionResult.IsFailure)
            return issuePositionResult.Error;

        var newIssuePosition = new IssuePosition(IssueId, issuePositionResult.Value);

        return newIssuePosition;
    }

    public Result<IssuePosition, Error> MoveBack()
    {
        var issuePositionResult = Position.Back();
        if (issuePositionResult.IsFailure)
            return issuePositionResult.Error;

        var newIssuePosition = new IssuePosition(IssueId, issuePositionResult.Value);

        return newIssuePosition;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return IssueId;
        yield return Position;
    }
}