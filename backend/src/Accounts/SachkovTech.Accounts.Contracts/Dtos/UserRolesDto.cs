namespace SachkovTech.Accounts.Contracts.Dtos;

public class UserRolesDto
{
    public Guid RoleId { get; init; }
    public RoleDto Role { get; init; } = default!;
    
    public Guid UserId { get; init; }
    public UserDto User { get; init; } = default!;
}