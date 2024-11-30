﻿using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Lesson;

public class Lesson : SoftDeletableEntity<LessonId>
{
    //EF CORE
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Lesson(LessonId id) : base(id) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Guid ModuleId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Experience Experience { get; private set; }
    public Guid PreviewId { get; private set; }
    public Guid[] Tags { get; private set; }
    public Guid[] Issues { get; private set; }
    public Video Video { get; private set; }

    public Lesson(
        LessonId id,
        Guid moduleId,
        Title title,
        Description description,
        Experience experience,
        Video video,
        Guid previewId,
        Guid[] tags,
        Guid[] issues) : base(id)
    {
        ModuleId = moduleId;
        Title = title;
        Description = description;
        Experience = experience;
        Video = video;
        PreviewId = previewId;
        Tags = tags;
        Issues = issues;
    }

    /// <summary>
    /// Метод, который полностью обновляет урок
    /// </summary>
    /// <param name="title">Название</param>
    /// <param name="description">Описание</param>
    /// <param name="experience">Опыт за урок</param>
    /// <param name="video">Ссылка на видео</param>
    /// <param name="fileId">Ссылка на файл</param>
    /// <param name="tags">Список тегов к уроку</param>
    /// <param name="issues">Список задач к уроку</param>
    public void Update(
        Title title,
        Description description,
        Experience experience,
        Video video,
        Guid fileId,
        Guid[] tags,
        Guid[] issues)
    {
        Title = title;
        Description = description;
        Experience = experience;
        Video = video;
        PreviewId = fileId;
        Tags = tags;
        Issues = issues;
    }

    /// <summary>
    /// Добавить задачку к уроку
    /// </summary>
    /// <param name="issueId">Ссылка на задачу</param>
    /// <returns>Выполненную операцию либо ошибку, что такая задача уже есть</returns>
    public UnitResult<Error> AddIssue(Guid issueId)
    {
        if (Issues.Contains(issueId))
            return Errors.General.AlreadyExist();

        Issues = Issues.Append(issueId).ToArray();
        return UnitResult.Success<Error>();
    }

    /// <summary>
    /// Добавить тег к уроку
    /// </summary>
    /// <param name="tagId">Ссылка на тег</param>
    /// <returns>Выполненную операцию либо ошибку, что такой тег уже есть</returns>
    public UnitResult<Error> AddTag(Guid tagId)
    {
        if (Tags.Contains(tagId))
            return Errors.General.AlreadyExist();

        Tags = Tags.Append(tagId).ToArray();
        return UnitResult.Success<Error>();
    }

    /// <summary>
    /// Удалить тег у урока
    /// </summary>
    /// <param name="tagId">Ссылка на тег</param>
    /// <returns>Выполненную операцию либо ошибку, что такой тег отсутствует</returns>
    public UnitResult<Error> RemoveTag(Guid tagId)
    {
        if (!Tags.Contains(tagId))
            return Errors.General.NotFound(tagId, "tag");

        Tags = Tags.Where(id => id != tagId).ToArray();
        return UnitResult.Success<Error>();
    }

    /// <summary>
    /// Удалить задачу у урока
    /// </summary>
    /// <param name="issueId">Ссылка на задачу</param>
    /// <returns>Выполненную операцию либо ошибку, что такая задача отсутствует</returns>
    public UnitResult<Error> RemoveIssue(Guid issueId)
    {
        if (!Issues.Contains(issueId))
            return Errors.General.NotFound(issueId, "tag");

        Issues = Issues.Where(id => id != issueId).ToArray();
        return UnitResult.Success<Error>();
    }
}
