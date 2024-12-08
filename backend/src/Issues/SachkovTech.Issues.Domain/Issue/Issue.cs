using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Issue;

public class Issue : SoftDeletableEntity<IssueId>
{
    private List<FileId> _files = [];

    //ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Issue(IssueId id) : base(id){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Issue(
        IssueId id,
        Title title,
        Description description,
        LessonId? lessonId,
        ModuleId moduleId,
        Experience experience,
        IEnumerable<FileId>? files = null) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        ModuleId = moduleId;
        Experience = experience;
        _files = files?.ToList() ?? [];
    }
    
    public Experience Experience { get; private set; } = default!;
    
    public Title Title { get; private set; } = default!;
    
    public Description Description { get; private set; } = default!;

    public LessonId? LessonId { get; private set; }
    
    public ModuleId ModuleId { get; private set; }

    public IReadOnlyList<FileId> Files => _files;

    public void UpdateFiles(IEnumerable<FileId> files)
    {
        _files = files.ToList();
    }

    public UnitResult<Error> UpdateMainInfo(
        Title title,
        Description description,
        LessonId? lessonId,
        ModuleId moduleId,
        Experience experience)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        ModuleId = moduleId;
        Experience = experience;

        return Result.Success<Error>();
    }
}