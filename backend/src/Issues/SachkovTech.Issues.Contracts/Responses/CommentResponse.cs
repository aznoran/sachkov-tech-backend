namespace SachkovTech.Issues.Contracts.Responses;

public class CommentResponse
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public Guid IssueReviewId { get; init; }
    
    public string Message { get; init; }
    
    public DateTime CreatedAt { get; init; }
}