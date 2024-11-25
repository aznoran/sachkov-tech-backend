using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync<TId>(DomainEntity<TId> entity, CancellationToken cancellationToken) where TId : IComparable<TId>;
}