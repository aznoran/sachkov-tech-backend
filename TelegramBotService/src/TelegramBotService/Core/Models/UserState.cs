using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBotService.Core.Models;

public class UserState
{
    [BsonId]
    public long ChatId { get; init; }

    public required int State { get; init; }
}