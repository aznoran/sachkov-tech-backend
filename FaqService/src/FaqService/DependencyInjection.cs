using FaqService.Api.Contracts;
using FaqService.Extensions;
using FaqService.Features.Commands.Answer.ChangeRating;
using FaqService.Features.Commands.Answer.CreateAnswer;
using FaqService.Features.Commands.Answer.UpdateMainInfo;
using FaqService.Features.Commands.Post.CreatePost;
using FaqService.Features.Commands.Post.SelectSolution;
using FaqService.Features.Commands.Post.UpdatePostMainInfo;
using FaqService.Features.Commands.Post.UpdateRefsAndTags;
using FaqService.Features.Queries;
using FaqService.Infrastructure;
using FaqService.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;

namespace FaqService;

public static class DependencyInjection
{
    public static IServiceCollection AddLogging(
        this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        services.AddSerilog();

        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreatePostHandler>();
        services.AddScoped<PostSelectSolutionHandler>();
        services.AddScoped<UpdatePostMainInfoHandler>();
        services.AddScoped<UpdatePostRefAndTagsHandler>();
        services.AddScoped<DeletePostHandler>();
        
        services.AddScoped<CreateAnswerHandler>();
        services.AddScoped<IncreaseAnswerRatingHandler>();
        services.AddScoped<DecreaseAnswerRatingHandler>();
        services.AddScoped<UpdateAnswerMainInfoHandler>();
        services.AddScoped<DeleteAnswerHandler>();

        services.AddScoped<GetPostsWithPaginationAndFiltersHandler>();
        services.AddScoped<GetAnswersWithPaginationHandler>();
        services.AddScoped<GetAnswerAtPostByIdHandler>();
        
        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<PostsRepository>();

        return services;
    }

    public static IServiceCollection AddDatabaseSetup(this IServiceCollection services)
    {
        services.AddHostedService<DatabaseSetupService>();

        return services;
    }
    
}