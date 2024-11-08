using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Lesson;

public class Lesson : SoftDeletableEntity<LessonId>
{
    private Lesson(LessonId id) : base(id) { }

    private List<Guid> _tags { get; set; }
    private List<Guid> _issues { get; set; }

    public Guid ModuleId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Experience Experience { get; private set; }
    public Guid VideoId { get; private set; }
    public Guid FileId { get; private set; }
    
    public IReadOnlyList<Guid> Tags => _tags;
    public IReadOnlyList<Guid> Issues => _issues;

    public Lesson(
        LessonId id,
        Guid moduleId,
        Title title,
        Description description,
        Experience experience,
        Guid videoId,
        Guid fileId,
        IEnumerable<Guid> tags,
        IEnumerable<Guid> issues) : base(id)
    {
        ModuleId = moduleId;
        Title = title;
        Description = description;
        Experience = experience;
        VideoId = videoId;
        FileId = fileId;
        _tags = tags.ToList();
        _issues = issues.ToList();
    }

    /// <summary>
    /// Метод, который полностью обновляет данные об уроке
    /// </summary>
    /// <param name="title">Название урока</param>
    /// <param name="description">Описание урока</param>
    /// <param name="experience">Количество опыта за просмотр урока</param>
    /// <param name="videoId">Ссылка на видео</param>
    /// <param name="fileId">Ссылка на название видео</param>
    /// <param name="tags">Список тегов к уроку</param>
    /// <param name="issues">Список задач к уроку</param>
    public void Update(
        Title title, 
        Description description, 
        Experience experience, 
        Guid videoId, 
        Guid fileId, 
        IEnumerable<Guid> tags,
        IEnumerable<Guid> issues)
    {
        Title = title;
        Description = description;
        Experience = experience;
        VideoId = videoId;
        FileId = fileId;
        _tags = tags.ToList();
        _issues = issues.ToList();
    }

    /// <summary>
    /// Метод, который добавляет ссылку на задачу в список.
    /// </summary>
    /// <param name="issueId">Ссылка на задачу</param>
    public UnitResult<Error> AddIssue(Guid issueId)
    {
        if (_issues.Contains(issueId))
            return Errors.General.AlreadyExist();
        
        _issues.Add(issueId);
        return UnitResult.Success<Error>();
    }
    
    /// <summary>
    /// Метод, который добавляет тег к уроку.
    /// </summary>
    /// <param name="tagId">Ссылка на тег</param>
    public UnitResult<Error> AddTag(Guid tagId)
    {
        if (_tags.Contains(tagId))
            return Errors.General.AlreadyExist();
        
        _tags.Add(tagId);
        return UnitResult.Success<Error>();
    }
    
    
}
