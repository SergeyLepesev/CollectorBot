using CollectorBot.Data;
using CollectorBot.Extension;
using CollectorBot.Model.DataBase;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = CollectorBot.Model.DataBase.User;

namespace CollectorBot.TelegramCommands
{
    public class IGaveCommand : ITelegramCommand
    {
        private const int ReferNameIndex = 0;
        private const int AmountIndex = 1;
        private const int StartCommentIndex = 2;
        private const int MinimumArgumentsCount = 3;

        private readonly IRepositoryAsync<User> _userRepository;
        private readonly IRepositoryAsync<Transaction> _transactionRepository;
        private readonly ITelegramBotClient _bot;

        public string Name => "igave"; //сообщаешь боту "я дал [сергей] [300] [отсоси у тракториста]" 

        public IGaveCommand(
            IRepositoryAsync<User> userRepository,
            IRepositoryAsync<Transaction> transactionRepository,
            ITelegramBotClient bot)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _bot = bot;
        }

        public async Task ExecuteAsync(Message message)
        {
            var owner = await _userRepository.GetItem(u => u.TelegramUserId == message.GetTelegramUserId());
            var args = message.Text.Remove(0, @"\igave ".Length).Split(' ').ToArray();

            switch (ValidateArgs(args))
            {
                case ValidationResult.InvalidArgsCount:
                {
                    await _bot.SendTextMessageAsync(owner.TelegramUserId, "Invalid number of arguments");
                    return;
                }
                case ValidationResult.InvalidAmount:
                {
                    await _bot.SendTextMessageAsync(owner.TelegramUserId, "Wrong argument 'Amount'");
                    return;
                }
            }

            var amount = GetAmount(args);
            var comment = GetComment(args);
            var refer = await _userRepository.GetItem(u => u.Name == GetReferName(args));
            if (refer is null)
            {
                await _bot.SendTextMessageAsync(owner.TelegramUserId, $"User with name {refer.Name} not found");
                return;
            }

            var transaction = new Transaction()
            {
                OwnerId = owner.Id,
                ReferId = refer.Id,
                Amount = amount,
                Comment = comment,
                Status = Status.Pending
            };

            try
            {
                await _transactionRepository.Create(transaction);
            }
            catch (System.Exception ex)
            {
                await _bot.SendTextMessageAsync(owner.TelegramUserId, ex.Message);
                return;
            }

            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Approve", $"/approve {transaction.Id}"),
                InlineKeyboardButton.WithCallbackData("Disapprove", $"/disapprove {transaction.Id}")
            });

            if (TransactionHelper.GetPendingTransactionAsync(_transactionRepository, transaction.Id) is null)
            {
                await _bot.SendTextMessageAsync(
                    refer.TelegramUserId,
                    $"New transaction {transaction.Id}:\n" +
                    $"{owner.Name} --{transaction.Amount}--> you.\n" +
                    $"{transaction.Comment}\n" +
                    $"Status: {transaction.Status}",
                    replyMarkup: keyboard);
            }

            await _bot.SendTextMessageAsync(owner.TelegramUserId, $"Command successfully completed! Wait for transaction confirmation.");
        }

        private ValidationResult ValidateArgs(string[] args)
        {
            if (args.Length < MinimumArgumentsCount)
            {
                return ValidationResult.InvalidArgsCount;
            }

            if (decimal.TryParse(args[AmountIndex].Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal amount))
            {
                return ValidationResult.InvalidAmount;
            }

            if(amount == 0)
            {
                return ValidationResult.InvalidAmount;
            }

            return ValidationResult.Valid;
        }

        private string GetReferName(string[] args) => args[ReferNameIndex];
        private decimal GetAmount(string[] args) => decimal.Parse(args[AmountIndex].Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture);
        private string GetComment(string[] args) => string.Join(' ', args.Skip(StartCommentIndex));

        private enum ValidationResult
        {
            Valid,
            InvalidArgsCount,
            InvalidAmount
        }
    }
}
