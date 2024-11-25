﻿using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Features.Lessons;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.Issues.Domain.Lesson;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.IntegrationTests.Lessons.GetLessonWithPaginationTests;

public class GetLessonWithPaginationTests : LessonsTestsBase
{
    public GetLessonWithPaginationTests(LessonTestWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandlerWithResult<PagedList<LessonResponse>, GetLessonsWithPaginationQuery>>();
    }

    private IQueryHandlerWithResult<PagedList<LessonResponse>, GetLessonsWithPaginationQuery> _sut;

    [Fact]
    public async Task Get_lessons_with_pagination()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var countLessons = 5;
        var lessons = await SeedLessonsToDatabase(WriteDbContext, countLessons, cancellationToken);

        var lessonIds = lessons.SelectMany(l => new[] { l.Video.FileId, l.PreviewId });
        Factory.SetupSuccessFileServiceMock(lessonIds);

        var page = 1;
        var pageSize = countLessons;
        var query = Fixture.CreateGetLessonsWithPaginationQuery(page, pageSize);

        // act
        var result = await _sut.Handle(query, cancellationToken);

        // assert
        result.IsSuccess.Should().BeTrue();
        var pagedList = result.Value;
        pagedList.Should().NotBeNull();
        pagedList.Items.Should().HaveCount(countLessons);
        pagedList.TotalCount.Should().Be(countLessons);
        pagedList.Items.First().VideoUrl.Should().Be("testUrl");
    }

    [Fact]
    public async Task Cant_get_lessons_with_invalid_query()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        
        var invalidPage = -1;
        var invalidPageSize = -1;
        var invalidQuery = new GetLessonsWithPaginationQuery(invalidPage, invalidPageSize);

        SetupFailureValidationResult(invalidQuery, cancellationToken);

        // act
        var result = await _sut.Handle(invalidQuery, cancellationToken);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Should().ContainSingle(e => e.Message == "Page is invalid");
        result.Error.Should().ContainSingle(e => e.Message == "PageSize is invalid");
    }

    private void SetupFailureValidationResult(
        GetLessonsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var validatorMock = Substitute.For<IValidator<GetLessonsWithPaginationQuery>>();

        var validationFailures = new List<ValidationFailure>
        {
            new(nameof(query.Page), Errors.General.ValueIsInvalid("Page").Serialize()),
            new(nameof(query.PageSize), Errors.General.ValueIsInvalid("PageSize").Serialize()),
        };
        var validationResult = new ValidationResult(validationFailures);
        validatorMock
            .ValidateAsync(query, cancellationToken)
            .Returns(validationResult);
    }

    private async Task<List<Lesson>> SeedLessonsToDatabase(
        IssuesWriteDbContext dbContext,
        int count,
        CancellationToken cancellationToken = default)
    {
        var lessons = Enumerable.Range(0, count).Select(_ => new Lesson(Guid.NewGuid(),
            Guid.NewGuid(),
            Title.Create("test title").Value,
            Description.Create("test description").Value,
            Experience.Create(1).Value,
            new Video(Guid.NewGuid()),
            Guid.NewGuid(),
            [Guid.NewGuid()],
            [Guid.NewGuid()])).ToList();

        await dbContext.Lessons.AddRangeAsync(lessons, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return lessons;
    }
}
