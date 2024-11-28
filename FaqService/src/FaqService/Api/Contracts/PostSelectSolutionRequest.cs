using FaqService.Features.Commands.Post.SelectSolution;

namespace FaqService.Api.Contracts;

public record PostSelectSolutionRequest(Guid AnswerId )
{
    public PostSelectSolutionCommand ToCommand(Guid Id) => new(Id, AnswerId);
}