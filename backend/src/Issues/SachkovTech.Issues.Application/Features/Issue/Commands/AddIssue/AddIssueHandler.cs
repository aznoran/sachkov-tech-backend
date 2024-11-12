using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

public class AddIssueHandler : ICommandHandler<Guid, AddIssueCommand>
{
    private readonly IIssueRepository _issuesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddIssueCommand> _validator;
    private readonly ILogger<AddIssueHandler> _logger;

    public AddIssueHandler(
        IIssueRepository issuesRepository,
        [FromKeyedServices(SharedKernel.Issues.Issues)] IUnitOfWork unitOfWork,
        IValidator<AddIssueCommand> validator,
        ILogger<AddIssueHandler> logger)
    {
        _issuesRepository = issuesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }
        
        var issue = InitIssue(command);
        
        await _issuesRepository.Add(issue, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Issue {issueId} was created",
            issue.Id);

        return issue.Id.Value;
    }

    private Domain.Issue.Issue InitIssue(AddIssueCommand command)
    {
        var issueId = IssueId.NewIssueId();
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var lessonId = LessonId.Empty;
        var experience = Experience.Create(command.Experience).Value;

        return new Domain.Issue.Issue(
            issueId,
            title,
            description,
            lessonId,
            experience);
    }
}