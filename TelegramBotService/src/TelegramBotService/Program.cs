using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Telegram.Bot;
using TelegramBotService.Abstractions;
using TelegramBotService.Factory;
using TelegramBotService.Handlers.Commands;
using TelegramBotService.Handlers.Messages;
using TelegramBotService.MongoDataAccess;
using TelegramBotService.Options;
using TelegramBotService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection(TelegramBotOptions.TELEGRAM_BOT));

builder.Services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        TelegramBotOptions? options = sp.GetService<IOptions<TelegramBotOptions>>()?.Value;
        ArgumentNullException.ThrowIfNull(options);
        TelegramBotClientOptions botOptions = new(options.BotToken);
        return new TelegramBotClient(botOptions, httpClient);
    });

builder.Services.AddSingleton<IMongoClient>
    (new MongoClient(builder.Configuration.GetConnectionString("MongoConnection")));

builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<IUserStateRepository, UserStateRepository>();

builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();

builder.Services.AddScoped<OnCommandHandler>();
builder.Services.AddScoped<LoginCommandHandler>();
builder.Services.AddScoped<HelpCommandHandler>();


builder.Services.AddScoped<IUserStateRepository, UserStateRepository>();
builder.Services.AddScoped<IUserStateProvider, UserStateProvider>();

builder.Services.AddScoped<IUserStateMachineFactory, UserStateMachineFactory>();
builder.Services.AddScoped<OnMessageHandler>();
builder.Services.AddScoped<UserStateFactory>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();