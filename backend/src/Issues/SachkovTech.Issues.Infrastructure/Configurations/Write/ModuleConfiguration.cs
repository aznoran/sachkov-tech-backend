using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.DataModels;
using SachkovTech.Issues.Contracts.Dtos;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("modules");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => ModuleId.Create(value));

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

        builder.Property(i => i.IssuesPosition)
            .ValueObjectsCollectionJsonConversion(
                valueObject => new IssuePositionDto
                {
                    IssueId = valueObject.IssueId,
                    Position = valueObject.Position
                },
                issuePosition => new IssuePosition(issuePosition.IssueId, Position.Create(issuePosition.Position).Value))
            .HasColumnName("issues_position");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.Property(m => m.DeletionDate)
            .IsRequired(false)
            .HasColumnName("deletion_date");

        builder.HasQueryFilter(f => f.IsDeleted == false);
    }
}