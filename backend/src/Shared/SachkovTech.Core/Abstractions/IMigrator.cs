namespace SachkovTech.Core.Abstractions;

public interface IMigrator
{
    Task Migrate(CancellationToken cancellationToken = default);
}
