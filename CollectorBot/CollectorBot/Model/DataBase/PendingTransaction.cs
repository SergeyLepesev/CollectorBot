namespace CollectorBot.Model.DataBase {
    public class PendingTransaction {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public bool OnPending { get; set; }
    }
}