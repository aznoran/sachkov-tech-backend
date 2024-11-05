using FileService.Data.Models;

namespace FileService.Data.Dtos;

public record GetLinkFileResult(FilePath FilePath, string Link);