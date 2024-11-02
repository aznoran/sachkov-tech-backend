using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Contracts.Dtos;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;
using SocialNetworkDto = SachkovTech.Accounts.Contracts.Dtos.SocialNetworkDto;

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

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            SecondName = user.SecondName,

            StudentAccount = user.StudentAccount is null
                ? null
                : new StudentAccountDto
                {
                    Id = user.StudentAccount.Id,
                    UserId = user.StudentAccount.UserId,
                    DateStartedStudying = user.StudentAccount.DateStartedStudying
                },

            SupportAccount = user.SupportAccount is null
                ? null
                : new SupportAccountDto
                {
                    Id = user.SupportAccount.Id,
                    UserId = user.SupportAccount.UserId,
                    AboutSelf = user.SupportAccount.AboutSelf
                },

            AdminAccount = user.AdminAccount is null
                ? null
                : new AdminAccountDto
                {
                    Id = user.AdminAccount.Id,
                    UserId = user.AdminAccount.UserId
                },

            SocialNetworks = user.SocialNetworks is null
                ? null
                : user.SocialNetworks
                    .Select(s => new SocialNetworkDto()
                    {
                        Name = s.Name,
                        Link = s.Link,
                    })
                    .ToList(),

            Roles = user.Roles is null
                ? null
                : user.Roles
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Permissions = r.Permissions is null
                            ? null
                            : r.Permissions.Select(p => new PermissionDto
                            {
                                Id = p.Id,
                                Code = p.Code
                            }).ToList()
                    })
                    .ToList()
        };

        return Result.Success<UserDto, ErrorList>(userDto);
    }
}