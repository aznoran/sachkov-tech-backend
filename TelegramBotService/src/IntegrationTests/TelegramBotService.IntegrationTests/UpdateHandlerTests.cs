using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Services;

namespace TelegramBotService.IntegrationTests;

public class UpdateHandlerTests : TelegramServiceTestsBase
{
    private const string ReceivedMessageTemplate = "Сообщение было получено: {0}";

    private readonly ILogger<UpdateHandler> _logger;
    private readonly ITelegramBotClient _botClient;

    public UpdateHandlerTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<UpdateHandler>>();
        _botClient = Scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
    }

    [Fact]
    public async Task Bot_respond_to_text_message()
    {
        // Arrange
        var update = CreateTestUpdate();

        var telegramServiceMock = Substitute.For<ITelegramService>();

        var sut = new UpdateHandler(telegramServiceMock, _logger);

        var expectedResponse = string.Format(ReceivedMessageTemplate, update.Message!.Text);

        // Act
        var token = new CancellationTokenSource().Token;
        await sut.HandleUpdateAsync(_botClient, update, token);

        // Assert
        await telegramServiceMock.Received(1).SendTextMessageAsync(
            Arg.Is<ChatId>(c => c.Identifier == update.Message!.Chat.Id),
            Arg.Is<string>(text => text == expectedResponse),
            Arg.Any<CancellationToken>());
    }
}