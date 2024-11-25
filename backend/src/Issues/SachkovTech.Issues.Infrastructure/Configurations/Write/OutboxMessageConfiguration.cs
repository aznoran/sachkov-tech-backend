using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Issues.Infrastructure.Outbox;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Payload)
            .HasColumnType("jsonb")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(o => o.Type)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasIndex(e => new
            {
                e.OccurredOnUtc,
                e.ProcessedOnUtc
            })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .IncludeProperties(e => new
            {
                e.Id,
                e.Type,
                Content = e.Payload
            })
            .HasFilter("processed_on_utc IS NULL");
    }
}