namespace BankApplication.Models
{
    public class Bank
    {
        public string BankId;
        public string BankName;
        public decimal SameBankRTGSCharge { get; set; } = 0;
        public decimal SameBankIMPSCharge { get; set; } = 5;
        public decimal OtherBankRTGSCharge { get; set; } = 2;
        public decimal OtherBankIMPSCharge { get; set; } = 6;
        public List<Account> Accounts { get; } = new List<Account>();
        public List<Currency> currencies = new List<Currency>();
        public Bank(string bankId, string bankName)
        {
            BankId = bankId;
            BankName = bankName;
            currencies.Add(new Currency("INR", 1));
        }
    }
}
