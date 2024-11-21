﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Issue;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Repositories;

public class IssuesesRepository : IIssuesRepository
{
    private readonly IssuesWriteDbContext _dbContext;
    
    public IssuesesRepository(IssuesWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Issue issue, CancellationToken cancellationToken = default)
    {
        await _dbContext.Issues.AddAsync(issue, cancellationToken);
        return issue.Id;
    }

    public Guid Save(Issue issue, CancellationToken cancellationToken = default)
    {
        _dbContext.Issues.Attach(issue);
        return issue.Id.Value;
    }

    public Guid Delete(Issue issue)
    {
        _dbContext.Issues.Remove(issue);

        return issue.Id;
    }

    public async Task<Result<Issue, Error>> GetById(
        IssueId issueId, CancellationToken cancellationToken = default)
    {
        var issue = await _dbContext.Issues
            .FirstOrDefaultAsync(m => m.Id == issueId, cancellationToken);

        if (issue is null)
            return Errors.General.NotFound(issueId);

        return issue;
    }

    public async Task<Result<Issue, Error>> GetByTitle(
        Title title, CancellationToken cancellationToken = default)
    {
        var issue = await _dbContext.Issues
            .FirstOrDefaultAsync(m => m.Title == title, cancellationToken);

        if (issue is null)
            return Errors.General.NotFound();

        return issue;
    }
}