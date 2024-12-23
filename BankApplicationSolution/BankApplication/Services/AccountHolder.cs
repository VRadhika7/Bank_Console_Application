using BankApplication.Models;
using BankApplication.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace BankApplication.Services
{
    public class AccountHolder : IAccountHolder
    {
        private readonly Account _account;
        private readonly List<Account> _allAccounts;
        private readonly List<Transaction> _transactions;
        private readonly Bank _bank;
        
        public AccountHolder(Account account, List<Account> allAccounts, List<Transaction> transactions,Bank bank)
        {
            _account = account;
            _allAccounts = allAccounts;
            _bank = bank;
            _transactions = transactions ?? new List<Transaction>();
        }

        private void LogTransaction(string accountId, string type, decimal amount)
        {
            _transactions.Add(new Transaction(accountId, _bank.BankId, type, amount));
        }
        public void ViewBalance(string accountId)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                Console.WriteLine($"Your current balance is: {account.Balance}");
            }
            else
            {
                Console.WriteLine($"Account with ID {accountId} not found.");
            }
        }
         public Account GetAccountById(string accountId)
         { 
            return _bank.Accounts.FirstOrDefault(a => a.AccountId == accountId);
         }
        public void DepositMoney(string accountId)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                Console.Write("Enter deposit amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    Console.Write("Enter currency (e.g., USD, EUR, INR): ");
                    string currency = Console.ReadLine().ToUpper();
                    Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>
            {
                { "USD", 83.0m }, 
                { "EUR", 90.0m }, 
                { "INR", 1.0m }
            };

                    if (exchangeRates.ContainsKey(currency))
                    {
                        decimal convertedAmount = amount * exchangeRates[currency];
                        account.Balance += convertedAmount;
                        LogTransaction(accountId, "Credit", convertedAmount);

                        Console.WriteLine($"Deposit successful! {amount} {currency} converted to {convertedAmount} INR.");
                        Console.WriteLine($"New balance for Account ID {accountId}: {account.Balance} INR");
                    }
                    else
                    {
                        Console.WriteLine("Unsupported currency. Deposit failed.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount. Deposit failed.");
                }
            }
            else
            {
                Console.WriteLine($"Account with ID {accountId} not found.");
            }
        }


        public void WithdrawMoney(string accountId)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                Console.Write("Enter withdrawal amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    if (amount > account.Balance)
                    {
                        Console.WriteLine("Insufficient funds.");
                    }
                    else
                    {
                        account.Balance -= amount;
                        LogTransaction(accountId, "Debit", amount);
                        Console.WriteLine($"Withdrawal successful! New balance for Account ID {accountId}: {account.Balance}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount. Withdrawal failed.");
                }
            }
            else
            {
                Console.WriteLine($"Account with ID {accountId} not found.");
            }
        }
        public void ViewTransactionHistory(string accountId)
        {
            var account = GetAccountById(accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine($"\n--- Transaction History for Account {accountId} ---");

            // Filter transactions for the given account
            var accountTransactions = _transactions?.Where(t => t.AccountId == accountId).ToList();

            if (accountTransactions == null || accountTransactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            foreach (var transaction in accountTransactions)
            {
                Console.WriteLine(transaction);
            }
        }

        public void TransferFunds()
        {
            Console.Write("Enter Sender Account ID: ");
            string senderAccountId = Console.ReadLine();
            Console.Write("Enter Receiver Account ID: ");
            string receiverAccountId = Console.ReadLine();
            Console.Write("Enter Transfer Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal transferAmount) || transferAmount <= 0)
            {
                Console.WriteLine("Invalid amount. Transfer failed.");
                return;
            }

            var senderAccount = GetAccountById(senderAccountId);
            var receiverAccount = GetAccountById(receiverAccountId);

            if (senderAccount == null || receiverAccount == null)
            {
                Console.WriteLine("Sender or Receiver account not found.");
                return;
            }

            bool isSameBank = senderAccount.BankId == receiverAccount.BankId;
            decimal rtgsCharge = isSameBank ? transferAmount * 0.00m : transferAmount * 0.02m;
            decimal impsCharge = isSameBank ? transferAmount * 0.05m : transferAmount * 0.06m;
            decimal totalCharges = rtgsCharge + impsCharge;
            decimal totalDeduction = transferAmount + totalCharges;
            if (senderAccount.Balance < totalDeduction)
            {
                Console.WriteLine("Insufficient funds in the sender's account.");
                return;
            }
            senderAccount.Balance -= totalDeduction;
            receiverAccount.Balance += transferAmount;
            LogTransaction(senderAccountId, "Debit - Transfer to " + receiverAccountId, transferAmount);
            LogTransaction(senderAccountId, "Debit - RTGS Service Charge", rtgsCharge);
            LogTransaction(senderAccountId, "Debit - IMPS Service Charge", impsCharge);
            LogTransaction(receiverAccountId, "Credit - Transfer from " + senderAccountId, transferAmount);
        }
    }
}
