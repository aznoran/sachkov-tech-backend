using EmailNotificationService.API;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.

services.Configure<MailOptions>(
    config.GetSection(MailOptions.SECTION_NAME));
services.AddScoped<EmailValidator>();
services.AddScoped<MailSender>();

Log.Logger = new LoggerConfiguration()
           .WriteTo.Console()
           .WriteTo.Debug()
           .WriteTo.Seq(config.GetConnectionString("Seq")
               ?? throw new ArgumentNullException("Seq connection string was not found"))
           .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)
           .CreateLogger();

services.AddSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("send", async (MailData mailData, MailSender mailSender) =>
    { 
        var result = await mailSender.Send(mailData);

        var response = result.IsSuccess ?
        new Response { StatusCode = 200, Success = true } :
        new Response { StatusCode = 400, Success = false, Message = result.Error };

        return response;
    });

app.UseHttpsRedirection();

app.Run();