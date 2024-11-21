using TagService;
using TagService.API;
using TagService.Extensions;
using TagService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHandlers();
builder.Services.AddScoped<ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.AddMigrations();
}

app.RegisterTagActions();
app.UseHttpsRedirection();

app.Run();