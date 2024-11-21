using CSharpFunctionalExtensions;

namespace NotificationService.Entities.ValueObjects;

public class MessageData : ComparableValueObject
{
    public string Title { get; } = null!;
    public string Message { get; } = null!;
    public MessageData(string title, string message)
    {
        Title = title;
        Message = message;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Title;
        yield return Message;
    }
}