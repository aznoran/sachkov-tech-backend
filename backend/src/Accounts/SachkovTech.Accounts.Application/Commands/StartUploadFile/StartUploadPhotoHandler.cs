using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Accounts.Domain;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Application.Commands.StartUploadFile;
public class StartUploadPhotoHandler : ICommandHandler<StartUploadPhotoResponse, StartUploadPhotoCommand>
{
    private readonly IFileService _fileHttpClient;
    private readonly UserManager<User> _userManager;

    public StartUploadPhotoHandler(IFileService fileHttpClient, UserManager<User> userManager)
    {
        _fileHttpClient = fileHttpClient;
        _userManager = userManager;
    }

    public async Task<Result<StartUploadPhotoResponse, ErrorList>> Handle(
        StartUploadPhotoCommand command,
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

        var startMultipartRequest = new StartMultipartUploadRequest(
            command.FileName,
            command.ContentType,
            command.FileSize);

        var result = await _fileHttpClient.StartMultipartUpload(
            startMultipartRequest,
            cancellationToken);

        if (result.IsFailure)
            return Errors.General.ValueIsInvalid(result.Error).ToErrorList();        

        return new StartUploadPhotoResponse(result.Value.FileId, result.Value.PresignedUrl);
    }
}
