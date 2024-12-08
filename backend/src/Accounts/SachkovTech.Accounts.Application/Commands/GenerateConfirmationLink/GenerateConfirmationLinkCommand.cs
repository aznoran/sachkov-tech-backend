using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.GenerateConfirmationLink;

public record GenerateConfirmationLinkCommand(
    Guid UserId) : ICommand;