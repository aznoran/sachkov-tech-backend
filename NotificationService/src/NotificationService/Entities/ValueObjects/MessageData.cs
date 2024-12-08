using CSharpFunctionalExtensions;

namespace NotificationService.Entities.ValueObjects;

public class MessageData : ComparableValueObject
{
    public string Title { get; }
    public string Template { get; }
    public string Data { get; }
    public MessageData(string title, string template, string data)
    {
        Title = title;
        Template = template;
        Data = data;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Title;
        yield return Template;
    }
}