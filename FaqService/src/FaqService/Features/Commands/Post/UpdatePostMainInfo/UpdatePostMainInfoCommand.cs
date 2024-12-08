namespace FaqService.Features.Commands.Post.UpdatePostMainInfo;

public record UpdatePostMainInfoCommand(Guid Id, string Title, string Description);