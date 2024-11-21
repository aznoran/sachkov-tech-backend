﻿using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Lesson;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Interfaces;

public interface ILessonsRepository
{
    public Task<Guid> Add(Lesson lesson, CancellationToken cancellationToken = default);

    public Guid Save(Lesson module);

    public Guid Delete(Lesson module);

    public Task<Result<Lesson, Error>> GetById(LessonId lessonId, CancellationToken cancellationToken = default);

    public Task<Result<Lesson, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default);
}
