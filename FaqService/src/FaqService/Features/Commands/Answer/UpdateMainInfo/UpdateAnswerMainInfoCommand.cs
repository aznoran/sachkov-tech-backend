namespace FaqService.Features.Commands.Answer.UpdateMainInfo;

public record UpdateAnswerMainInfoCommand(Guid PostId, Guid AnswerId, string Text);