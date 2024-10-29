using FileService.Data.Dtos;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Data.Documents
{
    public class FileDataDocument
    {
        [BsonId, BsonRepresentation(BsonType.String)]
        public Guid Id { get; init; }

        [BsonElement("name")]
        public string Name { get; init; }

        [BsonElement("storage_path")]
        public FilePath StoragePath { get; init; }

        [BsonElement("upload_date")]
        public DateTime UploadDate { get; init; } = DateTime.Now;

        [BsonElement("is_uploaded")]
        public bool IsUploaded { get; init; }

        [BsonElement("file_size")]
        public long FileSize { get; init; }

        [BsonElement("mime_type")]
        public string MimeType { get; init; }

        [BsonElement("owner_type")]
        public string OwnerType { get; init; }

        [BsonElement("owner_id"), BsonRepresentation(BsonType.String)]
        public Guid OwnerId { get; init; }

        [BsonElement("attributes")]
        IReadOnlyList<FileAttribute> Attributes { get; init; }

        public FileDataDocument()
        {
            
        }

        public FileDataDocument(
            string name,
            FilePath storagePath,
            bool isUploaded,
            long fileSize,
            string mimeType,
            string ownerType,
            Guid ownerId,
            IReadOnlyList<FileAttribute> attributes
            )
        {
            Id = Guid.NewGuid();
            Name = name;
            StoragePath = storagePath;
            UploadDate = DateTime.Now;
            IsUploaded = isUploaded;
            FileSize = fileSize;
            MimeType = mimeType;
            OwnerType = ownerType;
            OwnerId = ownerId;
            Attributes = attributes;
        }
    }
}
