using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Module.ValueObjects;

public class IssuePosition : ComparableValueObject, IPositionable
{
    public IssuePosition(IssueId issueId, Position position)
    {
        IssueId = issueId;
        Position = position;
    }

    public IssueId IssueId { get; }

    public Position Position { get; }

    public IPositionable Move(Position newPosition) => new IssuePosition(IssueId, newPosition);
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return IssueId;
        yield return Position;
    }
}