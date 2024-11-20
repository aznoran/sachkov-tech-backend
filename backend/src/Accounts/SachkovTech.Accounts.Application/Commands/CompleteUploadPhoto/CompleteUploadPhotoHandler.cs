using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Domain;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Application.Commands.CompleteUploadPhoto;
public class CompleteUploadPhotoHandler : ICommandHandler<CompleteUploadPhotoCommand>
{
    private readonly IFileService _fileHttpClient;
    private readonly UserManager<User> _userManager;

    public CompleteUploadPhotoHandler(
        IFileService fileHttpClient,
        UserManager<User> userManager)
    {
        _fileHttpClient = fileHttpClient;
        _userManager = userManager;
    }
    public async Task<UnitResult<ErrorList>> Handle(
        CompleteUploadPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = Photo.Validate(
            command.FileName,
            command.ContentType,
            command.FileSize);

        if (validateResult.IsFailure)
            return validateResult.Error.ToErrorList();

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user == null)
        {
            return Errors.General.NotFound(command.UserId, nameof(command.UserId)).ToErrorList();
        }

        var completeRequest = new CompleteMultipartRequest(command.UploadId, command.Parts);

        var result = await _fileHttpClient.CompleteMultipartUpload(completeRequest, cancellationToken);

        if (result.IsFailure)
        {            
            return Errors.General.ValueIsInvalid(result.Error).ToErrorList();
        }

        var photoResult = Photo.Create(result.Value.FileId);

        if (!photoResult.IsSuccess)
        {
            return photoResult.Error.ToErrorList();
        }

        user.Photo = photoResult.Value;

        return UnitResult.Success<ErrorList>();
    }
}