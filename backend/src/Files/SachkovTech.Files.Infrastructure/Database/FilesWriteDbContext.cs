using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Files.Domain;

namespace SachkovTech.Files.Infrastructure.Database;

public class FilesWriteDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public FilesWriteDbContext(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    public DbSet<FileData> FileData => Set<FileData>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("files");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FilesWriteDbContext).Assembly,
            type => type.FullName?.Contains("Database.Configurations.Write") ?? false);
    }
}