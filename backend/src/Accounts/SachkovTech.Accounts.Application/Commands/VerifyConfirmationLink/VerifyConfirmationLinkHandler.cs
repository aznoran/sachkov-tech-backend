using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Application.Commands.GenerateConfirmationLink;
using SachkovTech.Accounts.Application.Commands.Register;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Accounts.Domain;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.VerifyConfirmationLink;

public class VerifyConfirmationLinkHandler : ICommandHandler<VerifyConfirmationLinkCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<VerifyConfirmationLinkHandler> _logger;
    private readonly IValidator<VerifyConfirmationLinkCommand> _validator;

    public VerifyConfirmationLinkHandler(
        UserManager<User> userManager,
        ILogger<VerifyConfirmationLinkHandler> logger,
        IValidator<VerifyConfirmationLinkCommand> validator)
    {
        _userManager = userManager;
        _validator = validator;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        VerifyConfirmationLinkCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            _logger.LogError("Failed to find user with id {UserId}.", command.UserId);

            return Errors.General.NotFound(command.UserId, nameof(command.UserId)).ToErrorList();
        }

        var result = await _userManager.ConfirmEmailAsync(user, command.Code);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to verify user: {UserId}'s email.", command.UserId);

            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));

            return new ErrorList(errors);
        }
        
        _logger.LogInformation("Verified user: {UserId}'s email successfully.", command.UserId);

        return UnitResult.Success<ErrorList>();
    }
}