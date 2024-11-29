namespace FaqService.Features.Queries;

public record GetAnswerQuery(
    Guid? Cursor,
    int Limit = 10);