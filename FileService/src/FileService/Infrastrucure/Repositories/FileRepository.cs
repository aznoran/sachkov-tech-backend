using CSharpFunctionalExtensions;
using FileService.Data.Documents;
using FileService.Data.Models;
using MongoDB.Driver;

namespace FileService.Infrastrucure.Repositories
{
    public class FileRepository
    {
        private readonly IMongoCollection<FileDataDocument> _fileCollection;
        private readonly ILogger<FileRepository> _logger;

        public FileRepository(IConfiguration configuration, ILogger<FileRepository> logger, MongoClient client)
        {
            var database = client.GetDatabase("file_service");
            _fileCollection = database.GetCollection<FileDataDocument>("files");
            _logger = logger;
        }

        public async Task<Result<IEnumerable<Guid>,Error>> Add(IEnumerable<FileDataDocument> filesData)
        {
            try
            {
                await _fileCollection.InsertManyAsync(filesData);

                return filesData.Select(f => f.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to save file in database.");

                return Error.Failure("Repository.Add.File", "Fail to save file in database.");
            }
        }

        public async Task<Result<IEnumerable<FileDataDocument>, Error>> Get(IEnumerable<Guid> fileIds)
        {
            try
            {
                var files = await _fileCollection.Find(f => fileIds.Contains(f.Id)).ToListAsync();

                if(files.Count == 0)
                    return Error.NotFound("Repository.Get.File", "The file with this id was not found.");

                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to remove file from database.");

                return Error.Failure("Repository.Get.File", "Fail to get file from database.");
            }
        }

        public async Task<UnitResult<Error>> Remove(IEnumerable<Guid> fileIds)
        {
            try
            {
                var result = await _fileCollection.DeleteManyAsync(f => fileIds.Contains(f.Id));

                if(result.DeletedCount == 0)
                    return Error.NotFound("Repository.Remove.File", "The file with this id was not found.");

                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to remove file from database.");

                return Error.Failure("Repository.Remove.File", "Fail to remove file from database.");
            }
        }
    }
}