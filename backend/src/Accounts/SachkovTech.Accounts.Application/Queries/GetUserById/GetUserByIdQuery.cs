using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery;