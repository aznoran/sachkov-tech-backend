namespace FaqService.Api.Contracts;

public record DeleteAnswerCommand(Guid PostId, Guid AnswerId);