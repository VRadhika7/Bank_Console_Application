namespace BankApplication.Models
{
    public class Account
    {
        public string AccountId { get; }
        public string AccountHolderName { get; }
        public string MobileNumber { get; set; }
        public string AadharNumber { get; }
        public decimal Balance { get; set; }
        public string BankId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Account(string accountId, string accountHolderName, string mobileNumber,string aadharNumber,string bankId,string username, string password)
        {
            AccountId = accountId;
            AccountHolderName = accountHolderName;
            MobileNumber = mobileNumber;
            AadharNumber = aadharNumber;
            Balance = 0;
            BankId = bankId;
            Username = username;
            Password = password;
        }
        public override string ToString()
        {
            return $"Account ID: {AccountId}, Username: {Username}, Balance: {Balance}";
        }
    }
}

