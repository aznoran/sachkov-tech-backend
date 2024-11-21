using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Interfaces;

public interface IModulesRepository
{
    Task<Guid> Add(Module issue, CancellationToken cancellationToken = default);
    
    Guid Save(Module issue, CancellationToken cancellationToken = default);
    
    Guid Delete(Module issue);
    
    Task<Result<Module, Error>> GetById(ModuleId moduleId, CancellationToken cancellationToken = default);
    
    Task<Result<Module, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default);
}