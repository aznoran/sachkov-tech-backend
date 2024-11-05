using FileService.Infrastrucure.Abstractions;

namespace FileService.Application.Commands.UploadFiles;

public record UploadFilesCommand(string OwnerTypeName, Guid OwnerId, IFormFileCollection Files) : ICommand;