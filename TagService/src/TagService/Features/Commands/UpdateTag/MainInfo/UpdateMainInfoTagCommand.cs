namespace TagService.Features.Commands.UpdateTag.MainInfo;

public record UpdateMainInfoTagCommand(Guid TagId, string Name, string Description);