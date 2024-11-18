using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Accounts.Domain;
using SachkovTech.Accounts.Domain.ValueObjects;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.StartUploadFile;
public class StartUploadPhotoHandler : ICommandHandler<StartUploadPhotoResponse, StartUploadPhotoCommand>
{
    private readonly FileHttpClient _fileHttpClient;
    private readonly UserManager<User> _userManager;

    public StartUploadPhotoHandler(FileHttpClient fileHttpClient, UserManager<User> userManager)
    {
        _fileHttpClient = fileHttpClient;
        _userManager = userManager;
    }

    public async Task<Result<StartUploadPhotoResponse, ErrorList>> Handle(

        StartUploadPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var startMultipartRequest = new StartMultipartUploadRequest(
            command.Filename,
            command.ContentType,
            command.FileSize);

        var result = await _fileHttpClient.StartMultipartUpload(
            startMultipartRequest,
            cancellationToken);

        if (result.IsFailure)
            return Errors.General.ValueIsInvalid(result.Error).ToErrorList();

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user == null)
        {
            return Errors.General.NotFound(command.UserId, nameof(command.UserId)).ToErrorList();
        }

        var photoResult = Photo.Create(
            result.Value.FileId,
            user.Photo.FileName,
            user.Photo.ContentType,
            user.Photo.Size);

        if (!photoResult.IsSuccess)
        {
            return photoResult.Error.ToErrorList();
        }

        user.Photo = photoResult.Value;

        return new StartUploadPhotoResponse(result.Value.FileId, result.Value.PresignedUrl);
    }
}
