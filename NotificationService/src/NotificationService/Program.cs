using NotificationService.Extensions;
using NotificationService.Features.Commands;
using NotificationService.Features.Queries;
using NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AddNotificationSettingsHandler>();
builder.Services.AddScoped<PatchNotificationSettingsHandler>();
builder.Services.AddScoped<GetNotificationSettingsHandler>();

builder.Services.AddScoped<ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.AddMigrations();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();