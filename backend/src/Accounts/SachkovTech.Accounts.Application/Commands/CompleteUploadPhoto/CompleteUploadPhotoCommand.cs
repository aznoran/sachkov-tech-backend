﻿using FileService.Contracts;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Commands.CompleteUploadPhoto;
public record CompleteUploadPhotoCommand(
    Guid UserId,
    string FileName, 
    string ContentType, 
    long FileSize,
    string UploadId, 
    List<PartETagInfo> Parts) : ICommand;
