using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;


namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class LessonDtoConfiguration : IEntityTypeConfiguration<LessonDto>
{
    public void Configure(EntityTypeBuilder<LessonDto> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(l => l.Id)
            .HasName("id");

        builder.Property(l => l.ModuleId)
            .IsRequired()
            .HasColumnName("module_id");
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnName("title");

        builder.Property(d => d.Description)
            .IsRequired()
            .HasColumnName("description");
        
        builder.Property(b => b.Experience)
            .HasColumnName("experience");
        
        builder.Property(l => l.VideoId)
            .IsRequired()
            .HasColumnName("video_id");
        
        builder.Property(l => l.PreviewFileId)
            .IsRequired()
            .HasColumnName("preview_file_id");

        builder.Property(l => l.Tags)
            .HasColumnName("tags");

        builder.Property(l => l.Issues)
            .HasColumnName("issues");
    }
}
