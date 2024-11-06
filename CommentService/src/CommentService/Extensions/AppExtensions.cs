using CommentService.Features.Commands.AddComment;
using CommentService.Features.Commands.DeleteComment;
using CommentService.Features.Commands.UpdateComment.MainInfo;
using CommentService.Features.Commands.UpdateComment.RatingDecrease;
using CommentService.Features.Commands.UpdateComment.RatingIncrease;
using CommentService.Features.Queries.GetCommentByRelationId;
using CommentService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Extensions;

public static class AppExtensions
{
    public static async Task AddMigrations(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        await using var scoped = app.Services.CreateAsyncScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
    
    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<AddCommentHandler>();
        services.AddScoped<DeleteCommentHandler>();
        services.AddScoped<UpdateMainInfoCommentHandler>();
        services.AddScoped<UpdateRatingIncreaseCommentHandler>();
        services.AddScoped<UpdateRatingDecreaseCommentHandler>();
        services.AddScoped<GetCommentByRelationIdWithPaginationHandler>();
        services.AddScoped<GetCommentByRelationIdWithPaginationHandler>();
    }
}