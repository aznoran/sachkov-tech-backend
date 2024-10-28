using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        builder.ComplexProperty(a => a.FullName, fb =>
        {
            fb.Property(a => a.FirstName).IsRequired(false).HasColumnName("first_name");
            fb.Property(a => a.SecondName).IsRequired(false).HasColumnName("second_name");
        });

        builder.HasOne(u => u.StudentAccount)
            .WithOne(s => s.User)
            .HasForeignKey<StudentAccount>("user_id")
            .IsRequired(true);

        builder.HasOne(u => u.SupportAccount)
            .WithOne(s => s.User)
            .HasForeignKey<SupportAccount>("user_id")
            .IsRequired(true);
        
        // builder.HasOne(u => u.AdminAccount)
        //     .WithOne(s => s.User)
        //     .HasForeignKey<AdminAccount>("user_id")
        //     .IsRequired(true);
    }
}