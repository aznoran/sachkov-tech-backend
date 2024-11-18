using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Dtos;
using SachkovTech.Issues.Application.Interfaces;

namespace SachkovTech.Issues.Infrastructure.DbContexts;

public class IssuesReadDbContext : DbContext, IReadDbContext
{
    private readonly string _connectionString;
    
    public IssuesReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IQueryable<IssueDto> Issues => Set<IssueDto>();
    
    public IQueryable<ModuleDto> Modules => Set<ModuleDto>();
    
    public IQueryable<IssueReviewDto> IssueReviewDtos => Set<IssueReviewDto>();
    
    public IQueryable<CommentDto> Comments => Set<CommentDto>();
    
    public IQueryable<UserIssueDto> UserIssues => Set<UserIssueDto>();
    public IQueryable<LessonDto> Lessons => Set<LessonDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("issues");
        
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IssuesWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);

        modelBuilder.Entity<IssueDto>().HasQueryFilter(i => !i.IsDeleted);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}