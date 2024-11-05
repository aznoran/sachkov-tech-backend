using FileService.Application.Interfaces;
using FileService.Data.Dtos;
using FileService.Infrastrucure.Abstractions;
using FileService.Infrastrucure.Repositories;

namespace FileService.Application.Queries.GetLinkFiles;
public class GetLinkFilesHandler : IQueryHandler<IEnumerable<FileLinkDto>, GetLinkFilesQuery>
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<GetLinkFilesHandler> _logger;
    private readonly FileRepository _filesRepository;

    public GetLinkFilesHandler(
        IFileProvider fileProvider, 
        ILogger<GetLinkFilesHandler> logger, 
        FileRepository filesRepository)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _filesRepository = filesRepository;
    }

    public async Task<IEnumerable<FileLinkDto>> Handle(GetLinkFilesQuery query, CancellationToken cancellationToken = default)
    {
        var files = await _filesRepository.Get(query.FileIds, cancellationToken);

        if (files.IsFailure)
            return [];

        var getFileLinks = await _fileProvider.GetLinks(files.Value.Select(f => new FilePath(f.StoragePath)));

        var results = from link in getFileLinks
                      let id = files.Value.First(f => f.StoragePath == link.FilePath).Id
                      select new FileLinkDto(id, link.Link);

        return results.ToList();
    }
}