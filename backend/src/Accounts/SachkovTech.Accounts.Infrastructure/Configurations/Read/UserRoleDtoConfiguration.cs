using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Contracts.Dtos;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Read;

public class UserRoleDtoConfiguration : IEntityTypeConfiguration<UserRolesDto>
{
    public void Configure(EntityTypeBuilder<UserRolesDto> builder)
    {
        builder.ToTable("user_roles");
        
        builder.HasKey(u => new { u.UserId, u.RoleId });
    }
}