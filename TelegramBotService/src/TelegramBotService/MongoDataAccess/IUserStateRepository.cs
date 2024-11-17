using TelegramBotService.Core.Models;

namespace TelegramBotService.MongoDataAccess;

public interface IUserStateRepository
{
    Task Add(UserState userState, CancellationToken cancellationToken = default);
    Task Update(UserState userState, CancellationToken cancellationToken = default);
    Task<UserState> Get(long userId, CancellationToken cancellationToken = default);
}