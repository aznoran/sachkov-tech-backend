namespace SachkovTech.Accounts.Contracts.Dtos;

public class RolePermissionDto
{
    public Guid RoleId { get; init; }
    
    public RoleDto Role { get; init; }

    public Guid PermissionId { get; init; }
    
    public PermissionDto Permission { get; init; }
}