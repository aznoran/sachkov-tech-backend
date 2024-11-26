using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.Module;
public class Module : SoftDeletableEntity<ModuleId>
{
    // ef core
    // ReSharper disable once UnusedMember.Local
    private Module(ModuleId id) : base(id)
    {
    }

    public Module(ModuleId moduleId, Title title, Description description)
        : base(moduleId)
    {
        Title = title;
        Description = description;
    }

    public Title Title { get; private set; } = default!;

    public Description Description { get; private set; } = default!;

    public IReadOnlyList<IssuePosition> IssuesPosition = [];
    public IReadOnlyList<LessonPosition> LessonsPosition = [];

    public void UpdateIssuesPosition(IEnumerable<IssuePosition> issuesPosition)
    {
        IssuesPosition = issuesPosition.ToList();
    }

    public void UpdateLessonsPosition(IEnumerable<LessonPosition> lessonsPosition)
    {
        LessonsPosition = lessonsPosition.ToList();
    }

    public void UpdateMainInfo(Title title, Description description)
    {
        Title = title;
        Description = description;
    }

    public void AddIssue(IssueId issueId, Position position)
    {
        var newIssuePosition = new IssuePosition(issueId, position);

        var newIssuesPosition = new List<IssuePosition>(IssuesPosition)
        {
            newIssuePosition
        };

        UpdateIssuesPosition(newIssuesPosition);
    }

    public void AddLesson(LessonId lessonId, Position position)
    {
        var newLessonPosition = new LessonPosition(lessonId, position);

        var newLessonsPosition = new List<LessonPosition>(LessonsPosition)
        {
            newLessonPosition
        };

        UpdateLessonsPosition(newLessonsPosition);
    }

    public UnitResult<Error> MoveIssue(IssuePosition issuePosition, int newPosition)
    {
        if(issuePosition.Position.Value == newPosition)
            return Result.Success<Error>();
        
        var rearrangedIssuesPositionResult = ChangePosition(
            IssuesPosition,
            issuePosition.Position.Value,
            newPosition);
        if (rearrangedIssuesPositionResult.IsFailure)
            return rearrangedIssuesPositionResult.Error;
        
        List<IssuePosition> rearrangedIssuesPosition = rearrangedIssuesPositionResult.Value
            .OfType<IssuePosition>()
            .ToList();
        
        if (rearrangedIssuesPosition.Count != IssuesPosition.Count)
        {
            return Errors.General.Failure();
        }
        
        UpdateIssuesPosition(rearrangedIssuesPosition);
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> MoveLesson(LessonPosition lessonPosition, Position newPosition)
    {
        if(lessonPosition.Position.Value == newPosition)
            return Result.Success<Error>();
        
        var rearrangedLessonsPositionResult = ChangePosition(
            LessonsPosition,
            lessonPosition.Position.Value,
            newPosition.Value);
        if (rearrangedLessonsPositionResult.IsFailure)
            return rearrangedLessonsPositionResult.Error;
        
        List<LessonPosition> rearrangedLessonsPosition = rearrangedLessonsPositionResult.Value
            .OfType<LessonPosition>()
            .ToList();
        
        if (rearrangedLessonsPosition.Count != LessonsPosition.Count)
            return Errors.General.Failure();        
        UpdateLessonsPosition(rearrangedLessonsPosition);
        return Result.Success<Error>();
    }
    /// <summary>
    /// Generic method to change position for IPositionable in collection 
    /// </summary>
    /// <param name="items"></param>
    /// <param name="positionFrom"></param>
    /// <param name="positionTo"></param>
    /// <returns></returns>
    private Result<List<IPositionable>,Error> ChangePosition(
        IReadOnlyList<IPositionable> items, int positionFrom, int positionTo)
    {
        if (positionFrom == positionTo 
            || positionFrom < 1 
            || positionTo < 1 
            || positionFrom > items.Count 
            || positionTo > items.Count)
        {
            return Errors.General.ValueIsInvalid(nameof(Position)); // No adjustment needed or invalid positions
        }
        
        var increment = positionFrom > positionTo ? 1 : -1;
        var start = int.Min(positionFrom, positionTo);
        var end = int.Max(positionFrom, positionTo);
        
        List<IPositionable> rearrangedItems = [];
        
        foreach (var currentItem in items)
        {
            var currentPosition = currentItem.Position.Value;
            
            if (currentPosition >= start && currentPosition <= end)
            {
                if (currentPosition == positionFrom)
                {
                    var newItem = currentItem.Move(Position.Create(positionTo).Value);
                    rearrangedItems.Add(newItem);
                }
                else
                {
                    var newItem = currentItem.Move(
                        Position.Create(currentItem.Position.Value + increment).Value);
                    rearrangedItems.Add(newItem);
                }
            }
            else
            {
                rearrangedItems.Add(currentItem);
            }
        }
        rearrangedItems = rearrangedItems.OrderBy(i => i.Position.Value).ToList();
        return rearrangedItems;
    }

    public UnitResult<Error> DeleteLessonPosition(LessonPosition lessonPosition)
    {
        var copiedList = LessonsPosition.Cast<IPositionable>().ToList();
        var updateListResult = DeleteItemFromIPositionableCollection(copiedList, lessonPosition);
        if (updateListResult.IsFailure)
            return updateListResult.Error;
        
        List<LessonPosition> rearrangedList = updateListResult.Value
            .OfType<LessonPosition>()
            .ToList();
        if (rearrangedList.Count != (LessonsPosition.Count - 1))
            return Errors.General.Failure();  
        
        UpdateLessonsPosition(rearrangedList);
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> DeleteIssuePosition(Guid issueId)
    {
        var issuePosition = IssuesPosition.FirstOrDefault(x => x.IssueId == issueId);
        if (issuePosition == null)
            return Errors.General.NotFound();
        
        var copiedList = IssuesPosition.Cast<IPositionable>().ToList();
        var updateListResult = DeleteItemFromIPositionableCollection(copiedList, issuePosition);
        if (updateListResult.IsFailure)
            return updateListResult.Error;
        
        List<IssuePosition> rearrangedList = updateListResult.Value
            .OfType<IssuePosition>()
            .ToList();
        if (rearrangedList.Count != (IssuesPosition.Count - 1))
            return Errors.General.Failure();  
        
        UpdateIssuesPosition(rearrangedList);
        return Result.Success<Error>();
    }
    private Result<List<IPositionable>,Error> DeleteItemFromIPositionableCollection(List<IPositionable> items, IPositionable itemToDelete)
    {
        var result = items.Remove(itemToDelete);
        if(result == false)
            return Errors.General.Failure();
        
        items = items.OrderBy(i => i.Position.Value).ToList();
        
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].Position.Value < itemToDelete.Position.Value)
                continue;
            items[i]=items[i].Move(Position.Create(i+1).Value);
        }
        return items;
    }
}

public interface IPositionable
{
    Position Position { get; }
    IPositionable Move(Position position);
    
}