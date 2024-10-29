using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Contracts.Dtos;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Read;

public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired(false);

        builder.Property(u => u.SecondName)
            .HasColumnName("second_name")
            .IsRequired(false);
        
        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<UserRolesDto>(
                e => e.HasOne<UserDto>()
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey("UserId") // Замените "UserId" на фактическое имя свойства в UserRolesDto, если необходимо
            );

        builder.HasOne(u => u.StudentAccount)
            .WithOne()
            .HasForeignKey<StudentAccountDto>(s => s.UserId);

        builder.HasOne(u => u.SupportAccount)
            .WithOne()
            .HasForeignKey<SupportAccountDto>(s => s.UserId);
        
        builder.HasOne(u => u.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccountDto>(s => s.UserId);
    }
}