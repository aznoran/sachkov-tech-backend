using NotificationService.Api;
using NotificationService.Extensions;
using NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpc();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddHandlers();

var app = builder.Build();

app.MapGrpcService<NotificationServiceGrpc>();

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