using MongoDB.Bson;
using MongoDB.Driver;
using TelegramBotService.Core.Models;

namespace TelegramBotService.MongoDataAccess;

public class UserStateRepository : IUserStateRepository
{
    private readonly MongoDbContext _dbContext;

    public UserStateRepository(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(UserState userState, CancellationToken cancellationToken = default)
    {
        await _dbContext.UserStates.InsertOneAsync(userState, cancellationToken: cancellationToken);
    }
    
    public async Task Update(UserState userState, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserState>.Filter.Eq(us => us.ChatId, userState.ChatId);

        var update = Builders<UserState>.Update
            .Set(us => us.State, userState.State);

        await _dbContext.UserStates.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<UserState> Get(long userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserStates.Find(f => f.ChatId == userId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}