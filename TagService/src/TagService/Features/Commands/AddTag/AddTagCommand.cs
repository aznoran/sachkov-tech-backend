namespace TagService.Features.Commands.AddTag;

public record AddTagCommand(string Name, string Description, DateTime CreatedAt, int UsagesCount);