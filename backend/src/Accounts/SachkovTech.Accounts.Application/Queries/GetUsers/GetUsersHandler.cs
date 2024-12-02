using SachkovTech.Accounts.Application.Database;
using SachkovTech.Accounts.Application.DataModels;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;

namespace SachkovTech.Accounts.Application.Queries.GetUsers;

public class GetUsersHandler : IQueryHandler<PagedList<UserDataModel>, GetUsersQuery>
{
    private readonly IAccountsReadDbContext _accountsReadDbContext;

    public GetUsersHandler(
        IAccountsReadDbContext accountsReadDbContext)
    {
        _accountsReadDbContext = accountsReadDbContext;
    }

    public async Task<PagedList<UserDataModel>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        var userQueryBuilder = new UserQueryBuilder(_accountsReadDbContext.Users);
        
        return await userQueryBuilder
            .IncludeAdminAccount()
            .IncludeStudentAccount()
            .IncludeSupportAccount()
            .IncludeRoles()
            .WithFirstName(query.FirstName)
            .WithSecondName(query.SecondName)
            .WithThirdName(query.ThirdName)
            .WithEmail(query.Email)
            .WithRole(query.Role)
            .WithRegistrationAfter(query.RegistrationDate)
            .SortByWithDirection(query.SortBy, query.SortDirection)
            .Build()
            .ToPagedList(query.Page, query.PageSize, cancellationToken);

        
    }
}