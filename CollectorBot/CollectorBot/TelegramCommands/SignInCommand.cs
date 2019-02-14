using System.Threading.Tasks;
using CollectorBot.Data;
using CollectorBot.Extension;
using CollectorBot.Model;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CollectorBot.Model.DataBase.User;

namespace CollectorBot.TelegramCommands {
    public class SignInCommand : ITelegramCommand {
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
            var messageStr = message.Text;
            var password = GetPassword(messageStr);
            var chatId = message.GetChatId();
            if (!ValidPassword(password)) {
                await _client.SendTextMessageAsync(chatId, "Wrond password");
                return;
            }

            var userName = GetUserName(messageStr);
            var user = await _userRepository.GetItem(u => u.Name == userName);
            if (user != null) {
                await _client.SendTextMessageAsync(chatId, "You already add");
                return;
            }

            var newUser = CreateNewUser(message);
            await _userRepository.Create(newUser);
            await _client.SendTextMessageAsync(chatId, $"User with name = {newUser.Name} success added");
        }

        private User CreateNewUser(Message message) {
            return new User{
                Name = GetUserName(message.Text),
                TelegramUserId = message.From.Id
            };
        }

        private bool ValidPassword(string password) => password == _settings[password];
        private string GetPassword(string message) => message.Split(' ')[1];
        private string GetUserName(string message) => message.Split(' ')[2];
    }
}