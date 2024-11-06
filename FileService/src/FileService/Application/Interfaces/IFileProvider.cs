using CSharpFunctionalExtensions;
using FileService.Data.Dtos;
using FileService.Data.Models;

namespace FileService.Application.Interfaces;

public interface IFileProvider
{
    IAsyncEnumerable<Result<UploadFilesResult, Error>> UploadFiles(IEnumerable<UploadFileData> filesData, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<GetLinkFileResult>> GetLinks(IEnumerable<FilePath> filePaths, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFile(FilePath filePath, CancellationToken cancellationToken = default);
}