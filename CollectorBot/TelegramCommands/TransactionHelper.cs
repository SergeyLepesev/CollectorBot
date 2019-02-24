using CollectorBot.Data;
using CollectorBot.Model.DataBase;
using System.Threading.Tasks;

namespace CollectorBot.TelegramCommands
{
    public static class TransactionHelper
    {
        public static async Task<Transaction> GetPendingTransactionAsync(
            IRepositoryAsync<Transaction> transactionRepository, 
            string exeptTransactionId)
        {
            return await transactionRepository.GetItem(t => t.Status == Status.Pending && t.Id != exeptTransactionId);
        }
    }
}
