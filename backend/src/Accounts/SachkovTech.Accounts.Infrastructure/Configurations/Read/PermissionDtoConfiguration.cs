using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Contracts.Dtos;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Read;

public class PermissionDtoConfiguration: IEntityTypeConfiguration<PermissionDto>
{
    public void Configure(EntityTypeBuilder<PermissionDto> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(u => u.Id);
    }
}