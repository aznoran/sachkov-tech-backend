using FileService.Core.Models;

namespace FileService.Core.Dtos;

public record UploadFileData(Stream ContentStream, FilePath FilePath);

