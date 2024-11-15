namespace SachkovTech.Issues.Contracts.Responses
{
    public record IssueResponse(
        Guid Id,
        Guid? ModuleId,
        string Title,
        string Description,
        int? Position,
        Guid? LessonId,
        FileResponse[] Files
    )
    {
        // Дополнительный конструктор с параметрами
        public IssueResponse(Guid id, Guid? lessonId, Guid? moduleId, int position)
            : this(id, moduleId, string.Empty, string.Empty, position, lessonId, Array.Empty<FileResponse>())
        {
        }
    }


    public record FileResponse(
        Guid Id,
        string Link
    );
}