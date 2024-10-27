namespace SachkovTech.TelegramBot.Infrastructure.Abstractions;

public interface IReceiverService
{
    Task Receive(CancellationToken cancellationToken = default);
}