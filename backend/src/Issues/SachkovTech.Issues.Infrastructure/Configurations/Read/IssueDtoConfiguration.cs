using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class IssueDtoConfiguration : IEntityTypeConfiguration<IssueDto>
{
    public void Configure(EntityTypeBuilder<IssueDto> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Files)
            .HasConversion(
                values => EfCoreFluentApiExtensions.SerializeValueObjectsCollection(),
                json => EfCoreFluentApiExtensions.DeserializeDtoCollection<Guid>(json).ToArray());
    }
}