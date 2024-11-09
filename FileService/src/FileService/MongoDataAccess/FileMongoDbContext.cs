using FileService.Core;
using MongoDB.Driver;

namespace FileService.MongoDataAccess;

public class FileMongoDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("files_db");

    public IMongoCollection<FileData> Files => _database.GetCollection<FileData>("files");
}