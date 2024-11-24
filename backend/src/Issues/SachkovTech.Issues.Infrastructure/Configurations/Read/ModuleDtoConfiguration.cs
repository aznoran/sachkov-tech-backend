using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Issues.Application.DataModels;
using SachkovTech.Issues.Contracts.Dtos;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class ModuleDtoConfiguration : IEntityTypeConfiguration<ModuleDataModel>
{
    public void Configure(EntityTypeBuilder<ModuleDataModel> builder)
    {
        builder.ToTable("modules");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.IssuesPosition)
            .HasConversion(
                issues => string.Empty,
                json => JsonSerializer.Deserialize<IssuePositionDto[]>(json, JsonSerializerOptions.Default) ??
                        Array.Empty<IssuePositionDto>())
            .HasColumnType("jsonb");
    }
}