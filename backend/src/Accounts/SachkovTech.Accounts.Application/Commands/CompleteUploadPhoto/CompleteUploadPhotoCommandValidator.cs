using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.CompleteUploadPhoto;
public class CompleteUploadPhotoCommandValidator : AbstractValidator<CompleteUploadPhotoCommand>
{
    public CompleteUploadPhotoCommandValidator()
    {
        RuleFor(p => p.UserId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(p => p.UploadId).NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleForEach(p => p.Parts)
            .ChildRules(part =>
            {
                part.RuleFor(x => x.PartNumber).NotEmpty()
                    .WithError(Errors.General.ValueIsRequired());

                part.RuleFor(x => x.ETag).NotEmpty()
                    .WithError(Errors.General.ValueIsRequired());
            });
    }
}
