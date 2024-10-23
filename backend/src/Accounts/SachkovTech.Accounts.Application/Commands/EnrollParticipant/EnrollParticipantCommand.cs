using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.EnrollParticipant;

public record EnrollParticipantCommand(string Email) : ICommand;