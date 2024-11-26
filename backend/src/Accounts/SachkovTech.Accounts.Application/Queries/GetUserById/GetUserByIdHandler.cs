using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Contracts.Dtos;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Queries.GetUserById;

public class GetUserByIdHandler : IQueryHandlerWithResult<UserDto, GetUserByIdQuery>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IAccountsReadDbContext _accountsReadDbContext;

    public GetUserByIdHandler(
        ILogger<GetUserByIdHandler> logger,
        IAccountsReadDbContext accountsReadDbContext)
    {
        _logger = logger;
        _accountsReadDbContext = accountsReadDbContext;
    }

    public async Task<Result<UserDto, ErrorList>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _accountsReadDbContext.Users
            .Include(u => u.StudentAccount)
            .Include(u => u.SupportAccount)
            .Include(u => u.Roles)
            .Include(u => u.AdminAccount)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found", query.UserId);

            return Result.Failure<UserDto, ErrorList>(
                Errors.General.NotFound(query.UserId).ToErrorList());
        }

        return user;
    }
}