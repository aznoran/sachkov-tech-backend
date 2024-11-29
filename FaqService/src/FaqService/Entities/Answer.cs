using CSharpFunctionalExtensions;
using SharedKernel;

namespace FaqService.Entities;

public class Answer : Entity<Guid>
{
    private Answer(
        Guid id,
        Guid postId,
        string text,
        Guid userId) : base(id)
    {
        PostId = postId;
        Text = text;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsSolution { get; private set; } = false;
    public Guid PostId { get; private set; } = default!;
    public string Text { get; private set; } 
    public Guid UserId { get; private set; } = default!;
    public int Rating { get; private set; } = default;
    public DateTime CreatedAt { get; private set; } = default!;

    public static Result<Answer, Error> Create(
        Guid id,
        Guid postId,
        string text,
        Guid userId)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length >= Constants.MAX_TEXT_LENGTH)
            return Error.Validation("Text");
        return new Answer(id, postId, text, userId);
    }

    public UnitResult<Error> UpdateMainInfo(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length >= Constants.MAX_TEXT_LENGTH)
            return Error.Validation("Text");
        Text = text;
        return Result.Success<Error>();
    }

    public void ChangeIsSolution(bool isSolution)
    {
        IsSolution = isSolution;
    }
    
    public void IncreaseRating() => Rating++;
    public void DecreaseRating() => Rating--;
}