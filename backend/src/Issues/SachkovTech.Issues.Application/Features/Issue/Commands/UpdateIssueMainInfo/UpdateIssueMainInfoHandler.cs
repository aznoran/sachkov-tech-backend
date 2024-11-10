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

namespace SachkovTech.Issues.Application.Features.Issue.Commands.UpdateIssueMainInfo;

public class UpdateIssueMainInfoHandler : ICommandHandler<Guid, UpdateIssueMainInfoCommand>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateIssueMainInfoCommand> _validator;
    private readonly ILogger<UpdateIssueMainInfoHandler> _logger;

    public UpdateIssueMainInfoHandler(
        IIssueRepository issueRepository,
        [FromKeyedServices(SharedKernel.Issues.Issues)] IUnitOfWork unitOfWork,
        IValidator<UpdateIssueMainInfoCommand> validator,
        ILogger<UpdateIssueMainInfoHandler> logger)
    {
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateIssueMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var issueResult = await _issueRepository.GetById(command.IssueId, cancellationToken);
        if (issueResult.IsFailure)
            return Errors.General.NotFound(command.IssueId).ToErrorList();

        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var experience = Experience.Create(command.Experience).Value;
        var lessonId = LessonId.Empty();

        var updateResult = issueResult.Value.UpdateMainInfo(
            title,
            description,
            lessonId,
            experience);
        
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Issue main info was updated with id {issueId}",
            command.IssueId);

        //TODO: переделать
        return (Guid)issueResult.Value.Id;
    }
}