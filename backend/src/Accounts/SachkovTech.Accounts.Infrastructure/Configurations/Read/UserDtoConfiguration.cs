using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Contracts.Dtos;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Read;

public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id)
            .HasName("PK_users_id");

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired(false);

        builder.Property(u => u.SecondName)
            .HasColumnName("second_name")
            .IsRequired(false);
    }
}