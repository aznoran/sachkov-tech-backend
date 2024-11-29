﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;
using SachkovTech.Issues.Contracts.Lesson;
using SachkovTech.Issues.Domain.Issue.ValueObjects;
using SachkovTech.Issues.Domain.Lesson;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.IntegrationTests.Lessons.GetLessonByIdTests;

public class GetLessonByIdTest : LessonsTestsBase
{
    public GetLessonByIdTest(LessonTestWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandlerWithResult<LessonResponse, GetLessonByIdQuery>>();
    }

    private readonly IQueryHandlerWithResult<LessonResponse, GetLessonByIdQuery> _sut;

    [Fact]
    public async Task Get_existing_lesson_by_id()
    {
        // arrange

        var cancellationToken = new CancellationTokenSource().Token;

        var lesson = await SeedLessonToDatabase(WriteDbContext, cancellationToken);

        var query = Fixture.CreateGetLessonByIdQuery(lesson.Id);

        Factory.SetupSuccessFileServiceMock([lesson.PreviewId, lesson.Video.FileId]);
        
        // act
        var result = await _sut.Handle(query, cancellationToken);

        // assert
        result.IsSuccess.Should().BeTrue();
        var lessonResponse = result.Value;
        lessonResponse.Should().NotBeNull();
        lessonResponse.Id.Should().Be(query.LessonId);
        lessonResponse.VideoUrl.Should().Be("testUrl");
        lessonResponse.PreviewUrl.Should().Be("testUrl");
    }

    [Fact]
    public async Task Get_non_existing_lesson_should_return_not_found()
    {
        // arrange
        Factory.SetupFailureFileServiceMock();

        var cancellationToken = new CancellationTokenSource().Token;

        var lesson = await SeedLessonToDatabase(WriteDbContext, cancellationToken);

        var query = Fixture.CreateGetLessonByIdQuery(lesson.Id);
        
        // act
        var result = await _sut.Handle(query, cancellationToken);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle(e => e.Message == "record not found");
    }

    private async Task<Lesson> SeedLessonToDatabase(
        IssuesWriteDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var lesson = new Lesson(Guid.NewGuid(),
            Guid.NewGuid(),
            Title.Create("test title").Value,
            Description.Create("test description").Value,
            Experience.Create(1).Value,
            new Video(Guid.NewGuid()),
            Guid.NewGuid(),
            [Guid.NewGuid()],
            [Guid.NewGuid()]);
        WriteDbContext.Lessons.Add(lesson);

        await dbContext.Lessons.AddAsync(lesson, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return lesson;
    }
}
