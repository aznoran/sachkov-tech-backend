using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Module.ValueObjects;

public class LessonPosition: ComparableValueObject, IPositionable
{
    public LessonPosition(LessonId lessonId, Position position)
    {
        LessonId = lessonId;
        Position = position;
    }

    public LessonId LessonId { get; }

    public Position Position { get; }

    public IPositionable Move(Position newPosition) => new LessonPosition(LessonId, newPosition);

    public Result<LessonPosition, Error> MoveForward()
    {
        var lessonPositionResult = Position.Forward();
        if (lessonPositionResult.IsFailure)
            return lessonPositionResult.Error;

        var newLessonPosition = new LessonPosition(LessonId, lessonPositionResult.Value);

        return newLessonPosition;
    }

    public Result<LessonPosition, Error> MoveBack()
    {
        var lessonPositionResult = Position.Back();
        if (lessonPositionResult.IsFailure)
            return lessonPositionResult.Error;

        var newLessonPosition = new LessonPosition(LessonId, lessonPositionResult.Value);

        return newLessonPosition;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return LessonId;
        yield return Position;
    }
}