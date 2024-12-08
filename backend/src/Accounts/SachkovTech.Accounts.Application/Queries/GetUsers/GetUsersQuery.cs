﻿using SachkovTech.Core.Abstractions;

namespace SachkovTech.Accounts.Application.Queries.GetUsers;

public record GetUsersQuery(
    string? Role,
    string? FirstName,
    string? SecondName,
    string? ThirdName,
    string? Email,
    DateTime? RegistrationDate,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;