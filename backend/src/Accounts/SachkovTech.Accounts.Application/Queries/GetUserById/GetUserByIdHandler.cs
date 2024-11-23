using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Contracts.Dtos;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;
using SocialNetworkDto = SachkovTech.Accounts.Contracts.Dtos.SocialNetworkDto;

namespace SachkovTech.Accounts.Application.Queries.GetUserById;

public class GetUserByIdHandler : IQueryHandler<UserDto?, GetUserByIdQuery>
{
    private readonly IAccountsReadDbContext _accountsReadDbContext;

    public GetUserByIdHandler(
        IAccountsReadDbContext accountsReadDbContext)
    {
        _accountsReadDbContext = accountsReadDbContext;
    }

    public async Task<UserDto?> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var userQueryBuilder = new UserQueryBuilder(_accountsReadDbContext.Users);
        
        return await userQueryBuilder
            .Build()
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
    }
}