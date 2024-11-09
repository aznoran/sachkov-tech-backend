using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;

namespace FileService.MongoDataAccess;

public interface IFileRepository
{
    Task<Result<Guid, Error>> Add(FileData fileData, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<FileData>> Get(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
    Task<UnitResult<Error>> DeleteMany(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
}