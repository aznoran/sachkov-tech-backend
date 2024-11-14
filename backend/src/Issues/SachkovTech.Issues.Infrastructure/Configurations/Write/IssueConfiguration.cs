using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Domain.Issue;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);

        builder.ComplexProperty(b => b.Experience, eb =>
        {
            eb.Property(e => e.Value)
                .HasColumnName("experience")
                .IsRequired(true);
        });

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => IssueId.Create(value));
        
        builder.Property(i => i.ModuleId)
            .HasConversion(
                id => id.Value,
                value => ModuleId.Create(value))
            .IsRequired(false);

        builder.Property(i => i.LessonId)
            .IsRequired(false)
            .HasConversion(
                id => id.Value,
                value => LessonId.Create(value));
        
        builder.ComplexProperty(i => i.Experience,
            eb =>
            {
                eb.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("experience");
            });

        builder.ComplexProperty(m => m.Title, tb =>
        {
            tb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Title.MAX_LENGTH)
                .HasColumnName("title");
        });

        builder.ComplexProperty(m => m.Description, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Description.MAX_LENGTH)
                .HasColumnName("description");
        });

        builder.Property(i => i.Files)
            .ValueObjectsCollectionJsonConversion(
                fileId => fileId.Value,
                FileId.Create)
            .HasColumnName("files");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.Property(i => i.DeletionDate)
            .IsRequired(false)
            .HasColumnName("deletion_date");
    }
}