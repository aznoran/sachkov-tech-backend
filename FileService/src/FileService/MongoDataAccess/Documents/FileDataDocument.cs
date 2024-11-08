using MongoDB.Bson.Serialization.Attributes;

namespace FileService.MongoDataAccess.Documents;

public class FileDataDocument
{
    [BsonId]
    public Guid Key { get; init; }

    [BsonElement("prefix")]
    public string Prefix { get; init; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("upload_date")]
    public DateTime UploadDate { get; init; }

    [BsonElement("file_size")]
    public long FileSize { get; init; }

    [BsonElement("mime_type")]
    public string ContentType { get; init; } = string.Empty;

    [BsonElement("owner_type")]
    public string OwnerType { get; init; } = string.Empty;

    [BsonElement("owner_id")]
    public Guid OwnerId { get; init; }
}