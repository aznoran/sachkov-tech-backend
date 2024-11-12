using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Write;

public class StudentAccountConfiguration : IEntityTypeConfiguration<StudentAccount>
{
    public void Configure(EntityTypeBuilder<StudentAccount> builder)
    {
        builder.ToTable("student_accounts");
    }
}