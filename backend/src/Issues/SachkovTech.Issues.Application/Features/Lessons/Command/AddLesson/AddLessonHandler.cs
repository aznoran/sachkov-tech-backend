using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.Issues.Domain.Lesson;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;

public class AddLessonHandler(
    IReadDbContext readDbContext,
    IValidator<AddLessonCommand> validator,
    ILessonsRepository lessonsRepository,
    [FromKeyedServices(SharedKernel.Modules.Issues)] IUnitOfWork unitOfWork,
    ILogger<AddLessonHandler> logger) : ICommandHandler<Guid, AddLessonCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        AddLessonCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var isModuleExists
            = await readDbContext.Modules.FirstOrDefaultAsync(v => v.Id == command.ModuleId, cancellationToken);
        if (isModuleExists is null)
            return Errors.General.NotFound(command.ModuleId, "module").ToErrorList();
        
        var title = Title.Create(command.Title).Value;
        var isLessonExists = await lessonsRepository.GetByTitle(title, cancellationToken);
        if (isLessonExists.IsSuccess)
            return Errors.General.AlreadyExist().ToErrorList();

        var lesson = CreateLesson(command);
        await lessonsRepository.Add(lesson, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Added new lesson with {LessonId}", lesson.Id);

        return lesson.Id.Value;
    }

    private Lesson CreateLesson(AddLessonCommand command) =>
        new(LessonId.NewLessonId(),
            command.ModuleId,
            Title.Create(command.Title).Value,
            Description.Create(command.Description).Value,
            Experience.Create(command.Experience).Value,
            command.VideoId,
            command.PreviewId,
            command.Tags.ToArray(),
            command.Issues.ToArray());
}
