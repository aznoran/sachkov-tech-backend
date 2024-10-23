using SachkovTech.Accounts.Contracts.Dtos;

namespace SachkovTech.Accounts.Application;

public interface IReadDbContext
{
    IQueryable<UserDto> Users { get; }
    
    IQueryable<RoleDto> Roles { get; }
    
    IQueryable<StudentAccountDto> StudentAccounts { get; }
    
    IQueryable<SupportAccountDto> SupportAccounts { get; }
}