using CSharpFunctionalExtensions;

namespace NotificationService.Entities
{
    public class MessageData : ValueObject
    {
        public string Title { get; } = null!;
        public string Message { get; } = null!;
        public MessageData(string title, string message)
        {
            Title = title;
            Message = message;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Message;
        }
    }
}
