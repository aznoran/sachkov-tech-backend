namespace FaqService.Features.Commands.Answer.ChangeRating;

public record IncreaseAnswerRatingCommand(Guid PostId, Guid AnswerId);