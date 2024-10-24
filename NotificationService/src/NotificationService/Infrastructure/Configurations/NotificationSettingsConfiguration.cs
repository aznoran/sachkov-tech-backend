using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;

namespace NotificationService.Infrastructure.Configurations
{
    public class NotificationSettingsConfiguration : IEntityTypeConfiguration<NotificationSettings>
    {
        public void Configure(EntityTypeBuilder<NotificationSettings> builder)
        {
            builder.ToTable("notification_settings");

            builder.HasKey(x => x.Id);

            builder.Property(n => n.UserId);

            builder.Property(n => n.EmailAddress)
                .HasConversion(
                    push => push.value.ToString(),
                    pull => Email.Create(pull).Value
                );

            builder.Property(n => n.SendEmail);

            builder.Property(n => n.TelegramId)
                .IsRequired(false);

            builder.Property(n => n.SendTelegram);

            builder.Property(n => n.WebEndpoint)
                .IsRequired(false);

            builder.Property(n => n.SendWeb);
        }
    }
}
