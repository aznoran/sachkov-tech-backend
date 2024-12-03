﻿using CSharpFunctionalExtensions;
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

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return LessonId;
        yield return Position;
    }
}