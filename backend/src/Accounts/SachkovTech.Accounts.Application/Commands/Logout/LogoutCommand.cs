using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.Logout;

public record LogoutCommand(Guid RefreshToken) : ICommand;