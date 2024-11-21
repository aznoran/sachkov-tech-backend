namespace FaqService.Features.Commands.Answer.Delete;

public record DeleteAnswerCommand(Guid PostId, Guid AnswerId);