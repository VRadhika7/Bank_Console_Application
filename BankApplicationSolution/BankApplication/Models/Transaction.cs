namespace BankApplication.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public string AccountId { get; set; }
        public string Type { get; set; } 
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Transaction(string accountId,string bankId, string type, decimal amount)
        {
            TransactionId = "TXN"+ bankId + accountId+ DateTime.Now.ToString("yyyyMMdd");
            AccountId = accountId;
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {TransactionId}, Account: {AccountId}, Type: {Type}, Amount: {Amount:C}, Date: {Date}";
        }
    }
}
