using SachkovTech.Accounts.Application.DataModels;
using SachkovTech.Accounts.Contracts.Dtos;

namespace SachkovTech.Accounts.Application.Database;

public interface IAccountsReadDbContext
{
    IQueryable<UserDataModel> Users { get; }
    
    IQueryable<RoleDto> Roles { get; }
    
    IQueryable<StudentAccountDto> StudentAccounts { get; }
    
    IQueryable<SupportAccountDto> SupportAccounts { get; }
}