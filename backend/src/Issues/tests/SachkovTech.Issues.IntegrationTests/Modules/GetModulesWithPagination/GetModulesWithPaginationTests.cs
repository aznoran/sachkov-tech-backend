using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Features.Modules.Queries.GetModulesWithPagination;
using SachkovTech.Issues.Contracts.Module;

namespace SachkovTech.Issues.IntegrationTests.Modules.GetModulesWithPagination;

public class GetModulesWithPaginationTests: ModuleTestsBase
{
    public GetModulesWithPaginationTests(ModuleTestWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetModules_should_return_array_with_correct_number_of_items()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedModule();
        await SeedModule();
        await SeedModule();
        
        var query = new GetModulesWithPaginationQuery(null, 2, 2);
        
        var sut = Scope.ServiceProvider.GetRequiredService
            <IQueryHandler<PagedList<ModuleResponse>, GetModulesWithPaginationQuery>>();
        
        // Act
        var result = await sut.Handle(query, cancellationToken);

        //Assert
        result.Should().NotBeNull();

        result.PageSize.Should().Be(query.PageSize);
        result.Page.Should().Be(query.Page);
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task GetModules_WithTitleFilter_should_return_array_with_correct_number_of_items()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedModule();
        await SeedModule();
        await SeedModule();
        
        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(x => x.Id == moduleId, cancellationToken);
        if(module is null)
            throw new Exception($"Seeded Module {moduleId} not found, something wrong with DB");
        
        var query = new GetModulesWithPaginationQuery(module.Title, 1, 2);
        
        var sut = Scope.ServiceProvider.GetRequiredService
            <IQueryHandler<PagedList<ModuleResponse>, GetModulesWithPaginationQuery>>();
        
        // Act
        var result = await sut.Handle(query, cancellationToken);

        //Assert
        result.Should().NotBeNull();

        result.PageSize.Should().Be(query.PageSize);
        result.Page.Should().Be(query.Page);
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
    }
}