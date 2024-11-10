using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.UploadFilesToIssue;

public record UploadFilesToIssueCommand(Guid IssueId, IEnumerable<UploadFileDto> Files) : ICommand;