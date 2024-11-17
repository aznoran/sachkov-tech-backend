using MongoDB.Driver;
using TelegramBotService.Core.Models;

namespace TelegramBotService.MongoDataAccess;

public class MongoDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("telegram_bot_service");

    public IMongoCollection<UserState> UserStates => _database.GetCollection<UserState>("user_state");
}