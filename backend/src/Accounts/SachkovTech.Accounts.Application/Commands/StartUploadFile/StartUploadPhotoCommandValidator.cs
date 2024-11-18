using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.StartUploadFile;
public class StartUploadPhotoCommandValidator : AbstractValidator<StartUploadPhotoCommand>
{
    public StartUploadPhotoCommandValidator()
    {
        RuleFor(u => u.UserId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(u => u.Filename).NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(u => u.ContentType).NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(u => u.FileSize).NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}
