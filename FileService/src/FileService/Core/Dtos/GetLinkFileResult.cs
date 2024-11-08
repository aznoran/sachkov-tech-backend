using FileService.Core.Models;

namespace FileService.Core.Dtos;

public record GetLinkFileResult(FilePath FilePath, string Link);