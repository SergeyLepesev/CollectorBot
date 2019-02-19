using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectorBot.Extension;
using CollectorBot.TelegramCommands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CollectorBot.Services {
    public class TelegramCommandService {
        private readonly IEnumerable<ITelegramCommand> _commands;
        private readonly ITelegramBotClient _client;

        public TelegramCommandService(
                IEnumerable<ITelegramCommand> commands,
                ITelegramBotClient client) {
            _commands = commands;
            _client = client;
        }

        public async Task ExecuteCommand(Message message) {
            var command = _commands.SingleOrDefault(c =>
                message.Text.ToLower().Contains($"/{c.Name.ToLower()}")
            );

            if (command is null) {
                await _client.SendTextMessageAsync(
                    message.GetChatId(),
                    $"Command not found from message {message.Text}"
                );
                return;
            }

            await command.ExecuteAsync(message);
        }
    }
}