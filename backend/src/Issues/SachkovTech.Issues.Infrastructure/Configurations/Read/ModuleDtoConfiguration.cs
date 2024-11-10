using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class ModuleDtoConfiguration : IEntityTypeConfiguration<ModuleDto>
{
    public void Configure(EntityTypeBuilder<ModuleDto> builder)
    {
        builder.ToTable("modules");

        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.IssuesPosition)
            .HasConversion(
                values => EfCoreFluentApiExtensions.SerializeValueObjectsCollection(),
                json => EfCoreFluentApiExtensions.DeserializeDtoCollection<IssuePositionDto>(json).ToArray());
    }
}