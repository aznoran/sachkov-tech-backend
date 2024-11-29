using MassTransit;
using MassTransit.DependencyInjection;
using SachkovTech.Issues.Application;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Contracts.Messaging;
using SachkovTech.Issues.Infrastructure;
using SachkovTech.Web;
using SachkovTech.Web.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies(builder.Configuration);

var app = builder.Build();
app.Configure();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("message",
    async (Bind<IIssueMessageBus, IPublishEndpoint> publishEndpoint) =>
    {
        await publishEndpoint.Value.Publish(new IssueSentOnReviewEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
    });

app.Run();

namespace SachkovTech.Web
{
    public partial class Program;
}