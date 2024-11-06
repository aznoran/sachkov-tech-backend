using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Issues.Domain.IssuesReviews.Entities;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CommentId.Create(value));

        builder.ComplexProperty(i => i.UserId, ub =>
        {
            ub.Property(i => i.Value)
                .HasColumnName("user_id")
                .IsRequired();
        });

        builder.ComplexProperty(c => c.Message, cb =>
        {
            cb.Property(m => m.Value)
                .HasMaxLength(SharedKernel.Constants.Default.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("message")
                .IsRequired();
        });

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}