using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.VerifyConfirmationLink;

public record VerifyConfirmationLinkCommand(
    Guid UserId,
    string Code) : ICommand;