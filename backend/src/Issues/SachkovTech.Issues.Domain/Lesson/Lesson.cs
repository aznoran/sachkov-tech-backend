using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Lesson;

public class Lesson : SoftDeletableEntity<LessonId>
{
    private Lesson(LessonId id) : base(id){}
    public Guid ModuleId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Experience Experience { get; private set; }
    public Guid VideoId { get; private set; }
    public Guid PreviewFileId { get; private set; }
    public Guid[] Tags { get; private set; }
    public Guid[] Issues { get; private set; }

    public Lesson(
        LessonId id,
        Guid moduleId,
        Title title,
        Description description,
        Experience experience,
        Guid videoId,
        Guid previewFileId,
        Guid[] tags,
        Guid[] issues) : base(id)
    {
        ModuleId = moduleId;
        Title = title;
        Description = description;
        Experience = experience;
        VideoId = videoId;
        PreviewFileId = previewFileId;
        Tags = tags;
        Issues = issues;
    }

    /// <summary>
    /// Метод, который полностью обновляет урок
    /// </summary>
    /// <param name="title">Название</param>
    /// <param name="description">Описание</param>
    /// <param name="experience">Опыт за урок</param>
    /// <param name="videoId">Ссылка на видео</param>
    /// <param name="fileId">Ссылка на файл</param>
    /// <param name="tags">Список тегов к уроку</param>
    /// <param name="issues">Список задач к уроку</param>
    public void Update(
        Title title,
        Description description,
        Experience experience,
        Guid videoId,
        Guid fileId,
        Guid[] tags,
        Guid[] issues)
    {
        Title = title;
        Description = description;
        Experience = experience;
        VideoId = videoId;
        PreviewFileId = fileId;
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
}
