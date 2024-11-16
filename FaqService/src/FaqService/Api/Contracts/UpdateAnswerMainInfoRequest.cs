using FaqService.Entities;
using FaqService.Features.Commands.Answer.UpdateMainInfo;

namespace FaqService.Api.Contracts;

public record UpdateAnswerMainInfoRequest(
    string Text)
{
    public UpdateAnswerMainInfoCommand ToCommand(Guid PostId, Guid AnswerId) => new(PostId, AnswerId, Text);
}