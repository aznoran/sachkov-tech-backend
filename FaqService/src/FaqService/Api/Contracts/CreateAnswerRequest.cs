using FaqService.Features.Commands.Answer.CreateAnswer;

namespace FaqService.Api.Contracts;

public record CreateAnswerRequest(
    string Text,
    Guid UserId)
{
    public CreateAnswerCommand ToCommand(Guid PostId) => new(PostId, Text, UserId);
}