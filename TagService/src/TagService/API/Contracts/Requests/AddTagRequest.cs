using TagService.Features.Commands.AddTag;

namespace TagService.API.Contracts.Requests;

public record AddTagRequest(string Name, string Description, DateTime CreatedAt, int UsagesCount)
{
    public AddTagCommand ToCommand() => new(Name, Description, CreatedAt, UsagesCount);
}