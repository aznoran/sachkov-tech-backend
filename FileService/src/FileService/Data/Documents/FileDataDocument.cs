using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Data.Documents
{
    public class FileDataDocument
    {
        [BsonId]
        public Guid Id { get; init; }

        [BsonElement("name")]
        public string Name { get; init; } = string.Empty;

        [BsonElement("storage_path")]
        public string StoragePath { get; init; } = string.Empty;

        [BsonElement("upload_date")]
        public DateTime UploadDate { get; init; }

        [BsonElement("file_size")]
        public long FileSize { get; init; }

        [BsonElement("mime_type")]
        public string MimeType { get; init; } = string.Empty;

        [BsonElement("owner_type")]
        public string OwnerType { get; init; } = string.Empty;

        [BsonElement("owner_id")]
        public Guid OwnerId { get; init; }
    }
}