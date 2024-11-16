using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class ModuleDtoConfiguration : IEntityTypeConfiguration<ModuleDto>
{
    public void Configure(EntityTypeBuilder<ModuleDto> builder)
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