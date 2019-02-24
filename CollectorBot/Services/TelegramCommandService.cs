using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectorBot.Data;
using CollectorBot.Extension;
using CollectorBot.TelegramCommands;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CollectorBot.Model.DataBase.User;

namespace CollectorBot.Services {
    public class TelegramCommandService {
        private readonly IRepositoryAsync<User> _userRepository;
        private readonly IEnumerable<ITelegramCommand> _commands;
        private readonly ITelegramBotClient _bot;

        public TelegramCommandService(
                IRepositoryAsync<User> userRepository,
                IEnumerable<ITelegramCommand> commands,
                ITelegramBotClient bot) {
            _userRepository = userRepository;
            _commands = commands;
            _bot = bot;
        }

        public async Task ExecuteCommand(Message message) {
            var command = _commands.SingleOrDefault(c =>
                message.Text.ToLower().Contains($"/{c.Name.ToLower()}")
            );

            if (command is null) {
                await _bot.SendTextMessageAsync(
                    message.GetChatId(),
                    $"Command not found from message {message.Text}"
                );
                return;
            }

            await command.ExecuteAsync(message);
        }

        public async Task Process(Update update)
        {
            var callbackCommand = update.CallbackQuery.Data;
            var user = _userRepository.GetItems(u => u.TelegramUserId == update.CallbackQuery.Message.Chat.Id);
            var command = _commands.SingleOrDefault(c => callbackCommand.Contains($"/{c.Name}"));
            if (command != null)
            {
                await command.ExecuteAsync(update.CallbackQuery.Message);
            }
        }
    }
}