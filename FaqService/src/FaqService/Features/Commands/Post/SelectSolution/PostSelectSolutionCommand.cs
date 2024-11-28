namespace FaqService.Features.Commands.Post.SelectSolution;

public record PostSelectSolutionCommand(Guid PostId, Guid AnswerId);