namespace TelegramBotService.Abstractions;

public interface IReceiverService
{
    Task Receive(CancellationToken cancellationToken = default);

}