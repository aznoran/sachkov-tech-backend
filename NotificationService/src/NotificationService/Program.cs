using MassTransit;
using NotificationService.Consumers;
using NotificationService.Extensions;
using NotificationService.Infrastructure;
using SachkovTech.Accounts.Communication;

var builder = WebApplication.CreateBuilder(args);

//Я знаю что ты напишешь что оно итак подтягиваться поэтому я отвечу что так будет лучше
var configuration = builder.Configuration.AddJsonFile("appsettings.json").Build();

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

builder.Services.AddAccountHttpCommunication(configuration);

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