using FileService.Contracts;

namespace SachkovTech.Accounts.Application.Requests;
public record CompleteMultipartUploadRequest(
    FileMetadataRequest FileMetadata,
    string UploadId,
    List<PartETagInfo> Parts);


