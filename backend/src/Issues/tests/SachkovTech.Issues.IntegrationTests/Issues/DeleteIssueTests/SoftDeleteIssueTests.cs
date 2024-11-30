using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Issues.Application.Features.Issue.Commands.DeleteIssue.SoftDeleteIssue;

namespace SachkovTech.Issues.IntegrationTests.Issues.DeleteIssueTests;

public class SoftDeleteIssueTests : IssueTestsBase
{
    private readonly SoftDeleteIssueHandler sut;

    public SoftDeleteIssueTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        sut = Scope.ServiceProvider.GetRequiredService<SoftDeleteIssueHandler>();
    }

    [Fact]
    public async Task Soft_delete_issue_successfully()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var issueId = await SeedIssue();

        var command = Fixture.CreateDeleteIssueCommand(issueId);

        // act
        var result = await sut.Handle(command, cancellationToken);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var issue = await ReadDbContext.Issues
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.Id == result.Value, cancellationToken);

        issue.Should().NotBeNull();
        issue?.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Soft_delete_issue_successfully_when_issue_is_already_soft_deleted()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var issueId = await SeedSoftDeletedIssue();

        var command = Fixture.CreateDeleteIssueCommand(issueId);

        var sut = Scope.ServiceProvider.GetRequiredService<SoftDeleteIssueHandler>();

        // act
        var result = await sut.Handle(command, cancellationToken);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var issue = await ReadDbContext.Issues
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.Id == result.Value, cancellationToken);

        issue.Should().NotBeNull();
        issue?.IsDeleted.Should().BeTrue();
    }
}