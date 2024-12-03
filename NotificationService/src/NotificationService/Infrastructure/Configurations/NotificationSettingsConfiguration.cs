using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Entities;

namespace NotificationService.Infrastructure.Configurations;

public class NotificationSettingsConfiguration : IEntityTypeConfiguration<UserNotificationSettings>
{
    public void Configure(EntityTypeBuilder<UserNotificationSettings> builder)
    {
        builder.ToTable("notification_settings");

        builder.HasIndex(u => u.UserId).IsUnique();

        builder.HasKey(x => x.Id);
    }
}