using CollectorBot.Data;
using CollectorBot.Model.DataBase;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CollectorBot.Model.DataBase.User;

namespace CollectorBot.TelegramCommands
{
    public class ApproveCommand : ITelegramCommand
    {
        private const int TransactionIdIndex = 1;

        private readonly IRepositoryAsync<User> _userRepository;
        private readonly IRepositoryAsync<Transaction> _transactionRepository;
        private readonly ITelegramBotClient _bot;

        public string Name => "approve";

        public ApproveCommand(
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
            var transaction = await _transactionRepository.GetItem(t => t.Id == GetTransactionId(message.Text));
            transaction.Status = Status.Approved;
            transaction.ResolveDate = DateTime.Now;
            await _transactionRepository.Update(transaction, t => t.Id == transaction.Id);

            var owner = await _userRepository.GetItem(u => u.Id == transaction.OwnerId);
            var refer = await _userRepository.GetItem(u => u.Id == transaction.ReferId);
            await _bot.SendTextMessageAsync(refer.TelegramUserId, "Transaction approved");
            await _bot.SendTextMessageAsync(
                  owner.TelegramUserId,
                  $"Transaction {transaction.Id}:\n" +
                  $"you --{transaction.Amount}--> {refer.Name}.\n" +
                  $"{transaction.Comment}\n" +
                  $"Status: {transaction.Status}");
        }

        private string GetTransactionId(string text) => text.Split(' ')[TransactionIdIndex];
    }
}
