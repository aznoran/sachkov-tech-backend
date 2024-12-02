using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Accounts.Application.Database;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Queries.GetUserById;

public class GetUserByIdHandler : IQueryHandlerWithResult<UserResponse, GetUserByIdQuery>
{
    private readonly IAccountsReadDbContext _accountsReadDbContext;

    public GetUserByIdHandler(IAccountsReadDbContext accountsReadDbContext)
    {
        _accountsReadDbContext = accountsReadDbContext;
    }

    public async Task<Result<UserResponse, ErrorList>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _accountsReadDbContext.Users
            .Include(u => u.StudentAccount)
            .Include(u => u.SupportAccount)
            .Include(u => u.AdminAccount)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
            return Errors.General.NotFound(query.UserId).ToErrorList();

        return new UserResponse(
            user.Id,
            user.FirstName,
            user.SecondName,
            user.ThirdName,
            user.Email,
            user.RegistrationDate,
            user.SocialNetworks,
            user.StudentAccount,
            user.SupportAccount,
            user.AdminAccount,
            user.Roles);
    }
}