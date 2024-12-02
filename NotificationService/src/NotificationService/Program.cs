using MassTransit;
using NotificationService.Consumers;
using NotificationService.Extensions;
using NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<NotificationSettingsDbContext>();

builder.Services.AddHandlers();

builder.Services.AddNotificationService();

builder.Services.AddMassTransit(configure =>
{
    configure.SetKebabCaseEndpointNameFormatter();

    configure.AddConsumer<UserRegisteredEventConsumer>();

    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["RabbitMQ:Host"]!), h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]!);
            h.Password(builder.Configuration["RabbitMQ:Password"]!);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //await app.AddMigrations();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();