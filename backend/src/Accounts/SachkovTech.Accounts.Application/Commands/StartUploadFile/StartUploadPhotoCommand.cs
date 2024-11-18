using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.StartUploadFile;
public record StartUploadPhotoCommand(
    Guid UserId,
    string Filename, 
    string ContentType, 
    long FileSize) : ICommand;
