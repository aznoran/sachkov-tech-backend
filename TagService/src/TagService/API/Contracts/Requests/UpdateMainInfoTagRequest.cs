using TagService.Features.Commands.UpdateTag.MainInfo;

namespace TagService.API.Contracts.Requests;

public record UpdateMainInfoTagRequest(string Name, string Description)
{
    public UpdateMainInfoTagCommand ToCommand(Guid TagId) => new(TagId, Name, Description);
}