namespace FileService.Data.Dtos;

public record UploadFileData(Stream ContentStream, FilePath FilePath);

