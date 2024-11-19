using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateIssuePosition;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class UpdateIssuePositionTest : ModulesTestsBase
{
    private readonly Mock<ILogger<UpdateIssuePositionHandler>> _loggerMock = new();
    
    public UpdateIssuePositionTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Update_Issue_Position_Should_Move_Issue()
    {
        // act
        var validator = Scope.ServiceProvider.GetRequiredService<IValidator<UpdateIssuePositionCommand>>();
        var cancellationToken = new CancellationTokenSource().Token;

        var seedResult = await Seeding.AddModuleWithIssuesToDatabase(
            WriteDbContext,
            UnitOfWork,
            cancellationToken);

        var command = new UpdateIssuePositionCommand(seedResult.ModuleId, seedResult.IssueId, 2);

        var handler = new UpdateIssuePositionHandler(
            Repository,
            UnitOfWork,
            validator,
            _loggerMock.Object);

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