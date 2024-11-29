namespace FaqService.Features.Commands.Answer.ChangeRating;

public record DecreaseAnswerRatingCommand(Guid PostId, Guid AnswerId);