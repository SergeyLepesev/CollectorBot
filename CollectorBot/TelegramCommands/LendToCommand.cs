using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectorBot.Data;
using CollectorBot.Extension;
using Telegram.Bot;
using Telegram.Bot.Types;
using CollectorBot.Model.DataBase;
using User = CollectorBot.Model.DataBase.User;
using System;

namespace CollectorBot.TelegramCommands
{
    public class LendToCommand : ITelegramCommand
    {
        private const int RecipientNameIndex = 0;
        private const int AmountIndex = 1;
        private const int StartCommentIndex = 2;
        private const int MinimumArgumentsCount = 3;

        private readonly IRepositoryAsync<User> _userRepository;
        private readonly IRepositoryAsync<Transaction> _transactionRepository;
        private readonly ITelegramBotClient _bot;

        public string Name => "lendto";

        public LendToCommand(
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
            var senderId = message.GetTelegramUserId();
            var args = message.Text.Split(' ').Skip(1).ToArray();

            switch (ValidateArgs(args))
            {
                case ValidationResult.Valid:
                {
                    break;
                }
                case ValidationResult.InvalidArgsCount:
                {
                    await _bot.SendTextMessageAsync(senderId, "Invalid number of arguments");
                    return;
                }
                case ValidationResult.InvalidAmount:
                {
                    await _bot.SendTextMessageAsync(senderId, "Wrong argument 'Amount'");
                    return;
                }
            }

            var amount = GetAmount(args);
            var comment = GetComment(args);
            var recipientName = GetRecipientName(args);
            User recipient;
            User sender;
            try
            {
                recipient = await _userRepository.GetItem(user => user.Name == recipientName);
                sender = await _userRepository.GetItem(user => user.TelegramUserId == senderId);
            }
            catch (System.Exception ex)
            {
                await _bot.SendTextMessageAsync(senderId, ex.Message);
                return;
            }
            if (recipient is null)
            {
                await _bot.SendTextMessageAsync(senderId, $"User with name {recipientName} not found");
                return;
            }

            var transaction = new Transaction()
            {
                OwnerId = sender.Id,
                ReferId = recipient.Id,
                Amount = amount,
                Comment = comment,
                CreationDate = DateTime.Now,
                Status = Status.Pending
            };

            try
            {
                await _transactionRepository.Create(transaction);
            }
            catch (System.Exception ex)
            {
                await _bot.SendTextMessageAsync(senderId, ex.Message);
            }

            await _bot.SendTextMessageAsync(recipient.TelegramUserId, 
                $"New transaction:\n" +
                $"{sender.Name} --{amount} rub--> you.\n" +
                $"{comment}\n" +
                $"Awaiting confirmation.");

            await _bot.SendTextMessageAsync(senderId, $"Command successfully completed! Wait for transaction confirmation.");
        }

        private ValidationResult ValidateArgs(string[] args)
        {
            if (args.Length < MinimumArgumentsCount)
            {
                return ValidationResult.InvalidArgsCount;
            }

            if (decimal.TryParse(args[1], out decimal amount))
            {
                return ValidationResult.InvalidAmount;
            }

            return ValidationResult.Valid;
        }

        private enum ValidationResult
        {
            Valid,
            InvalidArgsCount,
            InvalidAmount
        }

        private string GetRecipientName(string[] args) => args[RecipientNameIndex];
        private decimal GetAmount(string[] args) => decimal.Parse(args[AmountIndex]);
        private string GetComment(string[] args)
        {
            var sb = new StringBuilder();
            for (int i = StartCommentIndex; i < args.Length; i++)
            {
                sb.Append(args[i]);
                sb.Append(' ');
            }

            return sb.ToString().TrimEnd(' ');
        }
    }
}
