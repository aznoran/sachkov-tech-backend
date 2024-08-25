﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SachkovTech.Infrastructure;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SachkovTech.Domain.Modules.Issue", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("module_id")
                        .HasColumnType("uuid")
                        .HasColumnName("module_id");

                    b.Property<Guid?>("parent_id")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "SachkovTech.Domain.Modules.Issue.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("LessonId", "SachkovTech.Domain.Modules.Issue.LessonId#LessonId", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid?>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("lesson_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Title", "SachkovTech.Domain.Modules.Issue.Title#Title", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("title");
                        });

                    b.HasKey("Id")
                        .HasName("pk_issues");

                    b.HasIndex("module_id")
                        .HasDatabaseName("ix_issues_module_id");

                    b.HasIndex("parent_id")
                        .HasDatabaseName("ix_issues_parent_id");

                    b.ToTable("issues", (string)null);
                });

            modelBuilder.Entity("SachkovTech.Domain.Modules.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "SachkovTech.Domain.Modules.Module.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Title", "SachkovTech.Domain.Modules.Module.Title#Title", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("title");
                        });

                    b.HasKey("Id")
                        .HasName("pk_modules");

                    b.ToTable("modules", (string)null);
                });

            modelBuilder.Entity("SachkovTech.Domain.Modules.Issue", b =>
                {
                    b.HasOne("SachkovTech.Domain.Modules.Module", null)
                        .WithMany("Issues")
                        .HasForeignKey("module_id")
                        .HasConstraintName("fk_issues_modules_module_id");

                    b.HasOne("SachkovTech.Domain.Modules.Issue", "ParentIssue")
                        .WithMany("SubIssues")
                        .HasForeignKey("parent_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("fk_issues_issues_parent_id");

                    b.OwnsOne("SachkovTech.Domain.Modules.FilesList", "FilesList", b1 =>
                        {
                            b1.Property<Guid>("IssueId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("IssueId");

                            b1.ToTable("issues");

                            b1.ToJson("files");

                            b1.WithOwner()
                                .HasForeignKey("IssueId")
                                .HasConstraintName("fk_issues_issues_id");

                            b1.OwnsMany("SachkovTech.Domain.Modules.File", "Files", b2 =>
                                {
                                    b2.Property<Guid>("FilesListIssueId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("PathToStorage")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("FilesListIssueId", "Id")
                                        .HasName("pk_issues");

                                    b2.ToTable("issues");

                                    b2.WithOwner()
                                        .HasForeignKey("FilesListIssueId")
                                        .HasConstraintName("fk_issues_issues_files_list_issue_id");
                                });

                            b1.Navigation("Files");
                        });

                    b.Navigation("FilesList");

                    b.Navigation("ParentIssue");
                });

            modelBuilder.Entity("SachkovTech.Domain.Modules.Issue", b =>
                {
                    b.Navigation("SubIssues");
                });

            modelBuilder.Entity("SachkovTech.Domain.Modules.Module", b =>
                {
                    b.Navigation("Issues");
                });
#pragma warning restore 612, 618
        }
    }
}