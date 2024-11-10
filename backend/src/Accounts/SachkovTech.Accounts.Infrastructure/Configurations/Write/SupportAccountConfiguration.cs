using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Accounts.Domain;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Infrastructure.Configurations.Write;

public class SupportAccountConfiguration : IEntityTypeConfiguration<SupportAccount>
{
    public void Configure(EntityTypeBuilder<SupportAccount> builder)
    {
        builder.ToTable("support_accounts");

        builder.Property(s => s.AboutSelf)
            .HasMaxLength(Constants.Default.MAX_HIGH_TEXT_LENGTH)
            .IsRequired();
    }
}