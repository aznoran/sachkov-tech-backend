using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

public class AddIssueHandler : ICommandHandler<Guid, AddIssueCommand>
{
    private readonly IIssuesRepository _issuesRepository;
    private readonly ILessonsRepository _lessonsRepository;
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddIssueCommand> _validator;
    private readonly ILogger<AddIssueHandler> _logger;

    public AddIssueHandler(
        IIssuesRepository issuesRepository,
        ILessonsRepository lessonsRepository,
        IModulesRepository modulesRepository,
        [FromKeyedServices(SharedKernel.Modules.Issues)]
        IUnitOfWork unitOfWork,
        IValidator<AddIssueCommand> validator,
        ILogger<AddIssueHandler> logger)
    {
        _issuesRepository = issuesRepository;
        _lessonsRepository = lessonsRepository;
        _modulesRepository = modulesRepository;
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

        LessonId? lessonId = null;

        if (command.LessonId is not null)
        {
            var lessonResult = await _lessonsRepository.GetById(command.LessonId, cancellationToken);
            if (lessonResult.IsFailure)
                return lessonResult.Error.ToErrorList();

            lessonId = LessonId.Create(lessonResult.Value.Id).Value;
        }

        var moduleResult = await _modulesRepository.GetById(command.ModuleId, cancellationToken);
        if (moduleResult.IsFailure)
            return moduleResult.Error.ToErrorList();

        var module = moduleResult.Value;
        var issue = InitIssue(module.Id, lessonId, command);
        await _issuesRepository.Add(issue, cancellationToken);

        module.AddIssue(issue.Id, Position.Create(module.IssuesPosition.Count + 1).Value);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Issue {issueId} was created",
            issue.Id);

        return issue.Id.Value;
    }

    private Domain.Issue.Issue InitIssue(
        ModuleId moduleId, 
        LessonId? lessonId,
        AddIssueCommand command)
    {
        var issueId = IssueId.NewIssueId();
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var experience = Experience.Create(command.Experience).Value;

        return new Domain.Issue.Issue(
            issueId,
            title,
            description,
            lessonId,
            moduleId,
            experience);
    }
}