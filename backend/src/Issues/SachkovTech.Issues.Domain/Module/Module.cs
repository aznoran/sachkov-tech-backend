using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Module;

public class Module : SoftDeletableEntity<ModuleId>
{
    // ef core
    private Module(ModuleId id) : base(id)
    {
    }

    public Module(ModuleId moduleId, Title title, Description description)
        : base(moduleId)
    {
        Title = title;
        Description = description;
    }

    public Title Title { get; private set; } = default!;

    public Description Description { get; private set; } = default!;

    public IReadOnlyList<IssuePosition> IssuesPosition = [];

    public void UpdateIssuesPosition(IEnumerable<IssuePosition> issuesPosition)
    {
        IssuesPosition = issuesPosition.ToList();
    }

    public void UpdateMainInfo(Title title, Description description)
    {
        Title = title;
        Description = description;
    }

    public UnitResult<Error> MoveIssue(IssuePosition issuePosition, Position newPosition)
    {
        var currentPosition = issuePosition.Position;

        if (currentPosition == newPosition || IssuesPosition.Count == 1)
            return Result.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var newIssuesPosition = MoveIssuesBetweenPositions(newPosition, currentPosition);
        if (newIssuesPosition.IsFailure)
            return newIssuesPosition.Error;

        UpdateIssuesPosition(newIssuesPosition.Value);

        return Result.Success<Error>();
    }

    private Result<List<IssuePosition>, Error> MoveIssuesBetweenPositions(
        Position newPosition,
        Position currentPosition)
    {
        var updatedPositions = IssuesPosition.ToList();

        var updatedIssue = updatedPositions
            .First(i => i.Position == currentPosition)
            .Move(newPosition);

        var updatedIssueIndex = updatedPositions
            .FindIndex(i => i.Position == currentPosition);

        if (newPosition < currentPosition)
        {
            for (int i = 0; i < updatedPositions.Count; i++)
            {
                var issue = updatedPositions[i];
                if (issue.Position >= newPosition && issue.Position < currentPosition)
                {
                    var moveResult = issue.MoveForward();
                    if (moveResult.IsFailure)
                        return moveResult.Error;

                    updatedPositions[i] = moveResult.Value;
                }
            }
        }
        else if (newPosition > currentPosition)
        {
            for (int i = 0; i < updatedPositions.Count; i++)
            {
                var issue = updatedPositions[i];
                if (issue.Position > currentPosition && issue.Position <= newPosition)
                {
                    var moveResult = issue.MoveBack();
                    if (moveResult.IsFailure)
                        return moveResult.Error;

                    updatedPositions[i] = moveResult.Value;
                }
            }
        }

        updatedPositions[updatedIssueIndex] = updatedIssue;

        updatedPositions = updatedPositions.OrderBy(i => i.Position.Value).ToList();

        return updatedPositions;
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= IssuesPosition.Count)
            return newPosition;

        var lastPosition = Position.Create(IssuesPosition.Count);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }


    private UnitResult<Error> RecalculatePositionOfOtherIssues(Position currentPosition)
    {
        if (currentPosition == IssuesPosition.Count)
            return Result.Success<Error>();

        var issuesToMove = IssuesPosition.Where(i => i.Position > currentPosition);
        foreach (var issue in issuesToMove)
        {
            var result = issue.MoveBack();
            if (result.IsFailure)
            {
                return result.Error;
            }
        }

        return Result.Success<Error>();
    }
}