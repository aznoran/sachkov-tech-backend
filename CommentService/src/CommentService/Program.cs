using CommentService.Api;
using CommentService.Extensions;
using CommentService.Features.Queries.GetCommentByRelationId;
using CommentService.HelperClasses;
using CommentService.Infrastructure;
using CommentService.Middlewares;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddHandlers();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.AddMigrations();
}

app.UseHttpsRedirection();

app.RegisterCommentActions();

app.Run();