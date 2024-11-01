using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Application.Commands.Register;

public record RegisterUserCommand(
    string Email, 
    string UserName, 
    string Password,
    FullNameDto? FullName,
    List<SocialNetworkDto> SocialNetwork) : ICommand;