using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CollectorBot.TelegramCommands {
    public interface ITelegramCommand {
        string Name { get; }
        Task ExecuteAsync(Message message);
    }
}