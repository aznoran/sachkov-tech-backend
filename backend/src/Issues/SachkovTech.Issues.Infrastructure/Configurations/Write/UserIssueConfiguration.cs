﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Issues.Domain.IssueSolving.Entities;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

public class UserIssueConfiguration : IEntityTypeConfiguration<UserIssue>
{
    public void Configure(EntityTypeBuilder<UserIssue> builder)
    {
        builder.ToTable("user_issues");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                push => push.Value,
                pull => UserIssueId.Create(pull));

        builder.Property(u => u.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder.ComplexProperty(u => u.IssueId, pb =>
        {
            pb.Property(a => a.Value)
                .IsRequired()
                .HasColumnName("issue_id");
        });

        builder.ComplexProperty(u => u.ModuleId, pb =>
        {
            pb.Property(a => a.Value)
                .IsRequired()
                .HasColumnName("module_id");
        });

        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.StartDateOfExecution);

        builder.Property(u => u.EndDateOfExecution);

        builder.ComplexProperty(u => u.Attempts, pb =>
        {
            pb.Property(a => a.Value)
                .IsRequired()
                .HasColumnName("attempts");
        });

        builder.ComplexProperty(u => u.PullRequestUrl, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("pull_request_url");
        });

    }
}