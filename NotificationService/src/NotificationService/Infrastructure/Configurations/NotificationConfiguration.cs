// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using NotificationService.Entities;
//
// namespace NotificationService.Infrastructure.Configurations;
//
// public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
// {
//     public void Configure(EntityTypeBuilder<Notification> builder)
//     {
//         builder.ToTable("notifications");
//
//         builder.HasKey(x => x.Id);
//
//         builder.Property(n => n.RoleIds)
//             .HasColumnType("uuid[]");
//
//         builder.Property(n => n.UserIds)
//             .HasColumnType("uuid[]");
//
//         builder.ComplexProperty(n => n.Message, ba =>
//         {
//             ba.Property(m => m.Message)
//                 .HasMaxLength(5000)
//                 .HasColumnName("message_value");
//
//             ba.Property(m => m.Title)
//                 .HasColumnName("message_title");
//         });
//
//         builder.Property(n => n.IsSend);
//
//         builder.Property(n => n.CreatedAt);
//
//         builder.Property(n => n.Status)
//             .HasConversion(
//                 push => push.ToString(),
//                 pull => Enum.Parse<NotificationStatusEnum>(pull)
//             );
//
//     }
// }