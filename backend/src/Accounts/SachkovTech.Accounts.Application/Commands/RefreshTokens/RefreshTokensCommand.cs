using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(Guid RefreshToken) : ICommand;