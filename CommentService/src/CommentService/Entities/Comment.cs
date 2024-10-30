using CommentService.HelperClasses;
using CSharpFunctionalExtensions;

namespace CommentService.Entities;

public class Comment : Entity<Guid>
{
    private Comment(
        Guid id,
        Guid relationId,
        Guid userId,
        Guid repliedId,
        string text,
        int rating) : base(id)
    {
        RelationId = relationId;
        UserId = userId;
        RepliedId = repliedId;
        Text = text;
        Rating = rating;
    }

    public Guid RelationId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid RepliedId { get; private set; }

    public string Text { get; private set; }

    public int Rating { get; private set; }

    public static Result<Comment, Error> Create(
        Guid relationId,
        Guid userId,
        Guid repliedId,
        string text,
        int rating)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > 5000)
            return Error.Validation("Text is invalid");

        return new Comment(Guid.NewGuid(), relationId, userId, repliedId, text, rating);
    }

    public void UpdateMainInfo(string text)
    {
        Text = text;
    }

    public void RatingIncrease()
    {
        Rating++;
    }

    public void RatingDecrease()
    {
        if (Rating > 0)
            Rating--;
    }
}