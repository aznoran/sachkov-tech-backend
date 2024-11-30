﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Issues.Application.Features.Issue.Commands.ForceDeleteIssue;

namespace SachkovTech.Issues.IntegrationTests.Issues.DeleteIssueTests;

public class ForceDeleteIssueTests : IssueTestsBase
{
    private readonly ForceDeleteIssueHandler sut;

    public ForceDeleteIssueTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        sut = Scope.ServiceProvider.GetRequiredService<ForceDeleteIssueHandler>();
    }

    [Fact]
    public async Task Force_delete_issue_successfully()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedModule();

        var issueId = await SeedIssue(moduleId);

        var command = Fixture.CreateDeleteIssueCommand(issueId);

        // act
        var result = await sut.Handle(command, cancellationToken);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var issue = await ReadDbContext.Issues
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.Id == result.Value, cancellationToken);

        issue.Should().BeNull();
    }
}