using CSharpFunctionalExtensions;
using FileService.Api.Contracts;
using FileService.Application.Interfaces;
using FileService.Data.Documents;
using FileService.Data.Dtos;
using FileService.Data.Models;
using FileService.Infrastrucure.Abstractions;
using FileService.Infrastrucure.Repositories;
using HeyRed.Mime;

namespace FileService.Application.Commands.UploadFiles;

public class UploadFilesHandler : ICommandHandler<UploadFilesResponse, UploadFilesCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<UploadFilesHandler> _logger;
    private readonly FileRepository _filesRepository;

    public UploadFilesHandler(
        IFileProvider fileProvider, 
        ILogger<UploadFilesHandler> logger, 
        FileRepository filesRepository)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _filesRepository = filesRepository;
    }

    public async Task<Result<UploadFilesResponse, ErrorList>> Handle(
        UploadFilesCommand command,
        CancellationToken cancellationToken = default)
    {
        List<Stream> fileContents = [];
        List<Guid> fileIds = [];

        var uploadFiles = new List<UploadFileData>();

        foreach (var file in command.Files)
        {

            var prefix = MimeTypesMap.GetMimeType(file.FileName).ToLower();

            fileContents.Add(file.OpenReadStream());

            var filePath = new FilePath(command.OwnerTypeName, file.FileName, prefix);

            uploadFiles.Add(new UploadFileData(fileContents.Last(), filePath));
        }

        var uploadAsyncEnum = _fileProvider.UploadFiles(uploadFiles, cancellationToken);

        await foreach (var uploadFileResult in uploadAsyncEnum)
        {
            if (uploadFileResult.IsFailure) continue;

            var uploadResult = uploadFileResult.Value;

            var saveFileResult = await SaveFile(uploadResult, command, cancellationToken);

            if (saveFileResult.IsSuccess)
                fileIds.Add(saveFileResult.Value);
        }

        if (fileIds.Count <= 0)
            return new ErrorList([Error.Failure("file.upload", "Fail to upload files in minio")]);

        var notUploadedFilesCount = command.Files.Count() - fileIds.Count;
        var result = new UploadFilesResponse(fileIds, fileIds.Count, notUploadedFilesCount);

        _logger.LogInformation("Files uploaded: {notUploadedFilesCount}", notUploadedFilesCount);

        foreach (var fileContent in fileContents)
            await fileContent.DisposeAsync();

        return result;
    }

    private async Task<Result<Guid, ErrorList>> SaveFile(
        UploadFilesResult uploadFileResult,
        UploadFilesCommand command,
        CancellationToken cancellationToken)
    {
        var fileData = new FileDataDocument(
            uploadFileResult.FileName,
            uploadFileResult.FilePath,
            true,
            uploadFileResult.FileSize,
            uploadFileResult.FilePath.Prefix,
            command.OwnerTypeName,
            command.OwnerId,
            []);

        var saveFileResult = await _filesRepository.Add([fileData], cancellationToken);

        if (saveFileResult.IsSuccess)
            return fileData.Id;

        await _fileProvider.RemoveFile(uploadFileResult.FilePath, cancellationToken);

        return saveFileResult.Error.ToErrorList();
    }
}
