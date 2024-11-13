using Microsoft.EntityFrameworkCore;
using TagService.Features.Commands.AddTag;
using TagService.Features.Commands.DeleteTag;
using TagService.Features.Commands.UpdateTag.MainInfo;
using TagService.Features.Commands.UpdateTag.UsagesDecrease;
using TagService.Features.Commands.UpdateTag.UsagesIncrease;
using TagService.Features.Queries.GetTagById;
using TagService.Features.Queries.GetTagsByIdsList;
using TagService.Infrastructure;

namespace TagService.Extensions;

public static class AppExtension
{
    public static async Task AddMigrations(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        await using var scope = app.Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<AddTagHandler>();
        services.AddScoped<DeleteTagHandler>();
        services.AddScoped<UpdateMainInfoTagHandler>();
        services.AddScoped<UpdateUsagesIncreaseTagHandler>();
        services.AddScoped<UpdateUsagesDecreaseTagHandler>();
        services.AddScoped<GetTagByIdHandler>();
        services.AddScoped<GetTagsByListIdsHandler>();
    }
}