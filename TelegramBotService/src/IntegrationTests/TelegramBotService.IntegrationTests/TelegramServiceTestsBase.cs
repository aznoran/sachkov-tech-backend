using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace TelegramBotService.IntegrationTests;

public abstract class TelegramServiceTestsBase : IClassFixture<IntegrationTestsWebAppFactory>, IAsyncDisposable
{
    protected readonly IntegrationTestsWebAppFactory Factory;
    protected IServiceScope Scope { get; private set; }
    protected readonly IFixture Fixture;

    protected TelegramServiceTestsBase(IntegrationTestsWebAppFactory factory)
    {
        Factory = factory;
        Fixture = new Fixture();
        Scope = factory.Services.CreateScope();
    }

    public async ValueTask DisposeAsync()
    {
        if (Scope is IAsyncDisposable scopeAsyncDisposable)
            await scopeAsyncDisposable.DisposeAsync();
        else
            Scope.Dispose();
    }

    protected Update CreateTestUpdate() =>
        new()
        {
            Message = new Message
            {
                Chat = new Chat
                {
                    Id = 1
                },
                Text = Fixture.Build<string>().Create(),
            }
        };
}