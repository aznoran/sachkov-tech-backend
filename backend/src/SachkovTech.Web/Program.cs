using MassTransit;
using MassTransit.DependencyInjection;
using SachkovTech.Accounts.Application.MessageBus;
using SachkovTech.Accounts.Contracts.Messaging;
using SachkovTech.Web;
using SachkovTech.Web.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies(builder.Configuration);

var app = builder.Build();
await app.Configure();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

namespace SachkovTech.Web
{
    public partial class Program;
}