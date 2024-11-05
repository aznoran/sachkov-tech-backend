using FileService.Infrastrucure.Abstractions;

namespace FileService.Application.Queries.GetLinkFiles;

public record GetLinkFilesQuery(IEnumerable<Guid> FileIds) : IQuery;