using FaqService.Features.Commands.Post.UpdatePostMainInfo;

namespace FaqService.Api.Contracts;

public record UpdatePostMainInfoRequest(
    string Title,
    string Description)
{
    public UpdatePostMainInfoCommand ToCommand(Guid Id) => new(Id, Title, Description);
}