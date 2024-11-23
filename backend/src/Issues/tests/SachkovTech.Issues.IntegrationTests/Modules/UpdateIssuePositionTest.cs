using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateIssuePosition;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class UpdateIssuePositionTest : ModulesTestsBase
{
    private readonly ILogger<UpdateIssuePositionHandler> _logger;
    private readonly IValidator<UpdateIssuePositionCommand> _validator;
    
    public UpdateIssuePositionTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<UpdateIssuePositionHandler>>();
        _validator = Scope.ServiceProvider.GetRequiredService<IValidator<UpdateIssuePositionCommand>>();
    }

    [Fact]
    public async Task Move_Issue_To_New_Position_In_Module()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;

        var seedResult = await Seeding.AddModuleWithIssuesToDatabase(
            WriteDbContext,
            UnitOfWork,
            cancellationToken);

        var command = new UpdateIssuePositionCommand(seedResult.ModuleId, seedResult.IssueId, 2);

        var handler = new UpdateIssuePositionHandler(
            Repository,
            UnitOfWork,
            _validator,
            _logger);

        // arrange
        var result = await handler.Handle(command, cancellationToken);
        
        // assert
        var module = await ReadDbContext.Modules
                .FirstOrDefaultAsync(m => m.Id == seedResult.ModuleId, cancellationToken);
        
        var issuePosition = module?.IssuesPosition
            .FirstOrDefault(i => i.IssueId == seedResult.IssueId);

        result.IsSuccess.Should().BeTrue();
        issuePosition?.Position.Should().Be(2);
    }
}