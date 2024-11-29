using FaqService.Features.Commands.Answer.ChangeRating;

namespace FaqService.Api.Contracts;

public record IncreaseAnswerRatingRequest()
{
    public IncreaseAnswerRatingCommand ToCommand(Guid PostId, Guid AnswerId) => new(PostId, AnswerId);
}