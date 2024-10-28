using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Queries.GetUserById;

public class GetUserByIdHandler : IQueryHandlerWithResult<UserResponse, GetUserByIdQuery>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IReadDbContext _readDbContext;

    public GetUserByIdHandler(
        ILogger<GetUserByIdHandler> logger,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<UserResponse, ErrorList>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _readDbContext.Users
            .Include(u => u.StudentAccount)
            .Include(u => u.SupportAccount)
            .Include(u => u.Roles)
            .Include(u => u.AdminAccount)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found", query.UserId);
            
            return Result.Failure<UserResponse, ErrorList>(
                Errors.General.NotFound(query.UserId).ToErrorList());
        }

        var studentAccountResponse = user.StudentAccount == null
            ? null
            : new StudentAccountResponse(
                user.StudentAccount.Id,
                user.StudentAccount.UserId,
                user.StudentAccount.SocialNetworks.Select(
                    x => new SocialNetworkResponse(x.Name, x.Link)).ToArray(),
                user.StudentAccount.DateStartedStudying);

        var supportAccountResponse = user.SupportAccount == null
            ? null
            : new SupportAccountResponse(
                user.SupportAccount.Id,
                user.SupportAccount.UserId,
                user.SupportAccount.SocialNetworks.Select(
                    x => new SocialNetworkResponse(x.Name, x.Link)).ToArray(),
                user.SupportAccount.AboutSelf);
        
        var adminAccountResponse = user.AdminAccount == null
            ? null
            : new AdminAccountResponse(
                user.AdminAccount.Id,
                user.AdminAccount.UserId);

        var response = new UserResponse(
            user.Id,
            user.FirstName,
            user.SecondName,
            studentAccountResponse,
            supportAccountResponse,
            adminAccountResponse,
            user.Roles.Select(x => new RoleResponse(x.Id, x.Name)).ToArray());

        return response;
    }
}