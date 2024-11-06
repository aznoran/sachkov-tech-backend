using CommentService.HelperClasses;
using CSharpFunctionalExtensions;

namespace CommentService.Entities;

public class Comment : Entity<Guid>
{
    public const int TEXT_MAX_LENGTH = 5000;

    private Comment(
        Guid id,
        Guid relationId,
        Guid userId,
        Guid? repliedId,
        string text,
        int rating) : base(id)
    {
        RelationId = relationId;
        UserId = userId;
        RepliedId = repliedId;
        Text = text;
        Rating = rating;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid RelationId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid? RepliedId { get; private set; }

    public string Text { get; private set; }

    public int Rating { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public static Result<Comment, Error> Create(
        Guid relationId,
        Guid userId,
        Guid? repliedId,
        string text,
        int rating)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > TEXT_MAX_LENGTH)
            return Error.Validation("Text is invalid");

        return new Comment(Guid.NewGuid(), relationId, userId, repliedId, text, rating);
    }

    public UnitResult<Error> Edit(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > TEXT_MAX_LENGTH)
            return Error.Validation("Text is invalid");

        Text = text;

        return Result.Success<Error>();
    }

    public void RatingIncrease() => Rating++;

    public void RatingDecrease() => Rating--;
}