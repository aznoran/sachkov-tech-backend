using FaqService.Features.Commands.Answer.ChangeRating;

namespace FaqService.Api.Contracts;

public record DecreaseAnswerRatingRequest()
{
    public DecreaseAnswerRatingCommand ToCommand(Guid PostId, Guid AnswerId) => new(PostId, AnswerId);
}