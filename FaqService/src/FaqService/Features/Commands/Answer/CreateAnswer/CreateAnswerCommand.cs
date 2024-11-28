namespace FaqService.Features.Commands.Answer.CreateAnswer;

public record CreateAnswerCommand(
    Guid PostId,
    string Text,
    Guid UserId);