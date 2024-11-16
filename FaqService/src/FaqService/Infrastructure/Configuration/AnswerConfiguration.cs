﻿using FaqService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace FaqService.Infrastructure.Configuration;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("answers");

        builder.HasKey(x => x.Id);

        builder.Property(a => a.PostId)
            .IsRequired()
            .HasColumnName("post_id");
        
        builder.Property(a => a.IsSolution)
            .IsRequired()
            .HasColumnName("is_solution");
        
        builder.Property(a => a.Text)
            .IsRequired()
            .HasMaxLength(Constants.MAX_TEXT_LENGTH)
            .HasColumnName("text");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder.Property(a => a.Rating)
            .IsRequired()
            .HasColumnName("rating");
        
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}