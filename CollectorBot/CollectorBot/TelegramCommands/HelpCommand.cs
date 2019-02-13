using System.Threading.Tasks;
using CollectorBot.Extension;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CollectorBot.TelegramCommands {
    public class HelpCommand : ITelegramCommand {
        private readonly ITelegramBotClient _client;

        public string Name => "help";

        public HelpCommand(ITelegramBotClient client) {
            _client = client;
        }

        public async Task ExecuteAsync(Message message) {
            await _client.SendTextMessageAsync(message.GetChatId(), $"hello { message.From.Username }");
        }
    }
}