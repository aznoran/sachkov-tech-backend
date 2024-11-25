using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Extensions;
using SachkovTech.Issues.Application.DataModels;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class IssueDtoConfiguration : IEntityTypeConfiguration<IssueDataModel>
{
    public void Configure(EntityTypeBuilder<IssueDataModel> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Files)
            .HasConversion(
                values => EfCoreFluentApiExtensions.SerializeValueObjectsCollection(),
                json => EfCoreFluentApiExtensions.DeserializeDtoCollection<Guid>(json).ToArray());
    }
}