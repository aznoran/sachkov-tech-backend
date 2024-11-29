using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using SachkovTech.Accounts.Infrastructure;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Infrastructure;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SachkovTech.Core.Options;
using SachkovTech.Accounts.Presentation;
using SachkovTech.Core.Abstractions;
using SachkovTech.Framework;
using SachkovTech.Framework.Models;
using SachkovTech.Accounts.Application;
using SachkovTech.Issues.Application;

namespace SachkovTech.Web;

public static class DependencyInjection
{
    public static void AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer()
            .AddSwaggerConfiguration()
            .AddAuthServices(configuration)
            .AddLogging(configuration)
            .AddApplicationLayers()
            .AddFramework()
            .AddAccountsModule(configuration)
            .AddIssuesModule(configuration);
    }

    private static IServiceCollection AddAccountsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccountsInfrastructure(configuration)
            .AddAccountsApplication(configuration)
            .AddAccountsPresentation();
        
        return services;
    }

    private static IServiceCollection AddIssuesModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIssuesApplication()
            .AddIssuesInfrastructure(configuration);
        
        return services;
    }

    private static IServiceCollection AddAuthServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing jwt configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });

        services.AddAuthorization();
        return services;
    }

    private static IServiceCollection AddLogging(
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

    private static IServiceCollection AddFramework(this IServiceCollection services)
    {
        services.AddHttpContextAccessor()
            .AddScoped<UserScopedData>();
        
        return services;
    }

    private static IServiceCollection AddApplicationLayers(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            typeof(SachkovTech.Accounts.Application.DependencyInjection).Assembly,
            typeof(SachkovTech.Issues.Application.DependencyInjection).Assembly,
        };

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }

    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    []
                }
            });
        });
        return services;
    }
}
