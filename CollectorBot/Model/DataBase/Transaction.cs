using System;

namespace CollectorBot.Model.DataBase {
    public class Transaction {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string ReferId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ResolveDate { get; set; }
        public Status Status { get; set; }
    }
}