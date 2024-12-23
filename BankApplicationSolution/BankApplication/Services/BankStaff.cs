using BankApplication.Models;
using BankApplication.Services.Interface;

namespace BankApplication.Services
{
    public class BankStaff : IBankStaff

    {
        private List<Transaction> _transactions = new List<Transaction>();
        private Dictionary<string, decimal> _accountBalances = new Dictionary<string, decimal>();
        private readonly Bank _bank;

        public BankStaff(Bank bank)
        {
            _bank = bank;
        }

        public void CreateAccount()
        {
            Console.WriteLine("\n--- Create New Account ---");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username and Password cannot be empty.");
                return;
            }
            Console.Write("Enter Account Holder Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Account holder name cannot be empty.");
                return;
            }

            Console.Write("Enter Mobile Number: ");
            string mobileNumber = Console.ReadLine();
            if (mobileNumber.Length != 10 || !mobileNumber.All(char.IsDigit))
            {
                Console.WriteLine("Invalid mobile number. Must be 10 digits.");
                return;
            }

            Console.Write("Enter Aadhar Number: ");
            string aadharNumber = Console.ReadLine();
            if (aadharNumber.Length != 12 || !aadharNumber.All(char.IsDigit))
            {
                Console.WriteLine("Invalid Aadhar number. Must be 12 digits.");
                return;
            }

            string accountId = name.Substring(0, 3).ToUpper() + DateTime.Now.ToString("yyyyMMdd") ;
            var account = new Account(accountId, name, mobileNumber, aadharNumber,_bank.BankId,username,password);
            _bank.Accounts.Add(account);

            Console.WriteLine($"Account created successfully! Account ID: {accountId}");
        }

        public void DeleteAccount()
        {
            Console.Write("Enter the Account ID to delete: ");
            string accountId = Console.ReadLine();

            var account = _bank.Accounts.FirstOrDefault(a => a.AccountId == accountId);

            if (account != null)
            {
                _bank.Accounts.Remove(account);
                Console.WriteLine($"Account with ID {accountId} has been deleted successfully.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        public void UpdateAccount()
        {
            Console.Write("Enter the Account ID to update: ");
            string accountId = Console.ReadLine();

            var account = _bank.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account != null)
            {
                Console.Write("Enter the new Mobile Number: ");
                string newMobileNumber = Console.ReadLine();
                if (newMobileNumber.Length != 10 || !newMobileNumber.All(char.IsDigit))
                {
                    Console.WriteLine("Invalid mobile number. Must be 10 digits.");
                    return;
                }

                account.MobileNumber = newMobileNumber;

                Console.WriteLine($"Account with ID {accountId} has been updated successfully.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        public void ViewAllAccounts()
        {
            if (_bank.Accounts == null || !_bank.Accounts.Any())
            {
                Console.WriteLine("No accounts available.");
                return;
            }
            else
            {
                Console.WriteLine("\n--- List of Accounts ---");
            }
            foreach (var account in _bank.Accounts)
            {
                Console.WriteLine($"Account ID: {account.AccountId}, Name: {account.AccountHolderName}, Balance: {account.Balance:C} INR");
            }
        }

        public void AddCurrency()
        {
            Console.Write("Enter Currency Code (e.g., USD): ");
            string code = Console.ReadLine();
            Console.Write("Enter Exchange Rate to INR: ");
            decimal rate = Convert.ToDecimal(Console.ReadLine());

            _bank.currencies.Add(new Currency(code, rate));
            Console.WriteLine($"Currency {code} added successfully with exchange rate {rate}.");
        }
        public void ViewTransactionHistory()
        {
            Console.WriteLine("\n--- Transaction History ---");
            if (_transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            foreach (var transaction in _transactions)
            {
                Console.WriteLine(transaction);
            }
        }


        public void LogTransaction(string accountid, string type, decimal amount)
        {
            var transaction = new Transaction(accountid, _bank.BankId, type, amount);
            _transactions.Add(transaction);
            Console.WriteLine("transaction logged successfully.");
        }

        public void DepositMoney(string accountId, decimal amount)
        {
            if (!_accountBalances.ContainsKey(accountId))
            {
                Console.WriteLine("Account not found.");
                return;
            }

            _accountBalances[accountId] += amount;
            LogTransaction(accountId, "Deposit", amount);
            Console.WriteLine($"Deposited {amount:C} to account {accountId}. New Balance: {_accountBalances[accountId]:C}");
        }

        public void WithdrawMoney(string accountId, decimal amount)
        {
            if (!_accountBalances.ContainsKey(accountId))
            {
                Console.WriteLine("Account not found.");
                return;
            }

            if (_accountBalances[accountId] < amount)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            _accountBalances[accountId] -= amount;
            LogTransaction(accountId, "Withdrawal", amount);
            Console.WriteLine($"Withdrew {amount:C} from account {accountId}. New Balance: {_accountBalances[accountId]:C}");
        }

        public void RevertTransaction()
        {
            Console.Write("Enter the Transaction ID: ");
            string transactionId = Console.ReadLine();

            var transaction = _transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            if (transaction == null)
            {
                Console.WriteLine("Transaction not found.");
                return;
            }

            var account = _bank.Accounts.FirstOrDefault(a => a.AccountId == transaction.AccountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            switch (transaction.Type)
            {
                case "Deposit":
                    account.Balance -= transaction.Amount;
                    LogTransaction(account.AccountId, "Revert Deposit", -transaction.Amount);
                    Console.WriteLine($"Reverted deposit of {transaction.Amount:C}.");
                    break;

                case "Withdrawal":
                    account.Balance += transaction.Amount;
                    LogTransaction(account.AccountId, "Revert Withdrawal", transaction.Amount);
                    Console.WriteLine($"Reverted withdrawal of {transaction.Amount:C}.");
                    break;

                default:
                    Console.WriteLine("Unknown transaction type. Cannot revert.");
                    return;
            }

            Console.WriteLine($"New Balance for Account {account.AccountId}: {account.Balance:C}");
        }

    }
}
