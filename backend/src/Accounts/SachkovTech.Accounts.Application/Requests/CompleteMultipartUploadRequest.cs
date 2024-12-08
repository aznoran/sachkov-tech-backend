using FileService.Contracts;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Application.Requests;
public record CompleteMultipartUploadRequest(
    FileMetadataRequest FileMetadata,
    string UploadId,
    List<PartETagInfo> Parts);


