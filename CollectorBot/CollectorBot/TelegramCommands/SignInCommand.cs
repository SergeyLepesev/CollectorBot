using System.Threading.Tasks;
using CollectorBot.Data;
using CollectorBot.Exception;
using CollectorBot.Extension;
using CollectorBot.Model;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CollectorBot.Model.DataBase.User;

namespace CollectorBot.TelegramCommands {
    public class SignInCommand : ITelegramCommand {
        private const int UserNameIndex = 2;
        private const int PasswordIndex = 1;

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
            catch (CollectorException ex) {
                await _client.SendTextMessageAsync(chatId, ex.Message);
            }
            
            await _client.SendTextMessageAsync(chatId, $"User with name = {newUser.Name} added successfully");
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