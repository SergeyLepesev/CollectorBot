using System.Threading.Tasks;
using CollectorBot.Data;
using CollectorBot.Extension;
using CollectorBot.Model;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CollectorBot.Model.DataBase.User;

namespace CollectorBot.TelegramCommands {
    public class SignInCommand : ITelegramCommand {
        private const int UserNameIndex = 2;
        private const int PasswordIndex = 1;
        private const int CountWordInCommand = 3;

        private readonly IRepositoryAsync<User> _userRepository;
        private readonly ITelegramBotClient _client;
        private readonly Settings _settings;

        public string Name => "signin";

        public SignInCommand(IRepositoryAsync<User> userRepository, ITelegramBotClient client, Settings settings) {
            _userRepository = userRepository;
            _client = client;
            _settings = settings;
        }

        public async Task ExecuteAsync(Message message) {
            if (ValidMessage(message.Text)) {
                await _client.SendTextMessageAsync(message.GetChatId(), "Command not valid");
            }

            var password = GetPassword(message.Text);
            var chatId = message.GetChatId();
            if (!ValidPassword(password)) {
                await _client.SendTextMessageAsync(chatId, "Wrong password");
                return;
            }
            var newUser = CreateNewUser(GetUserName(message.Text), message.GetTelegramUserId());
            try {
                await _userRepository.Create(newUser);
            }
            catch (System.Exception ex) {
                await _client.SendTextMessageAsync(chatId, ex.Message);
            }

            await _client.SendTextMessageAsync(chatId, $"User with name = {newUser.Name} added successfully");
        }

        private bool ValidMessage(string text) {
            var splitedText = text.Split(' ');
            if (splitedText.Length != CountWordInCommand) {
                return false;
            }

            return true;
        }

        private User CreateNewUser(string name, long telegramUserId) {
            return new User{
                Name = name,
                TelegramUserId = telegramUserId
            };
        }

        private bool ValidPassword(string password) => password == _settings["Password"];
        private string GetPassword(string message) => message.Split(' ')[PasswordIndex];
        private string GetUserName(string message) => message.Split(' ')[UserNameIndex];
    }
}