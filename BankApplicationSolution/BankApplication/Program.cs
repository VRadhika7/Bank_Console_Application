using BankApplication.Models;
using BankApplication.Services;
public class Program
{
    private readonly BankStaff _bankStaff;
    private readonly AccountHolder _accountHolder;

    // Constructor for Program class
    public Program(BankStaff bankStaff)
    {
        _bankStaff = bankStaff;
        _accountHolder = null;
    }

    public static void Main(string[] args)
    {
        Console.Write("Enter Bank Name: ");
        string bankName = Console.ReadLine();
        string bankId = bankName.Substring(0, 3).ToUpper() + DateTime.Now.ToString("yyyyMMdd");
        Bank currentBank = new Bank(bankId, bankName);
        BankStaff bankStaff = new BankStaff(currentBank);
        Program program = new Program(bankStaff);
        Console.WriteLine("Welcome to the Bank Account Simulation!");

        while (true)
        {
            Console.WriteLine("Select User Type:");
            Console.WriteLine("1. Bank Staff");
            Console.WriteLine("2. Account Holder");
            Console.WriteLine("3. Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    program.BankStaffOperations();
                    return;
                case "2":
                    Console.Write("Enter Account ID: ");
                    string accountId = Console.ReadLine();
                    program.SetAccountHolder(accountId, currentBank, program.Get_accountHolder());
                    return;
                case "3":
                    Console.WriteLine("Exiting the application.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private AccountHolder Get_accountHolder()
    {
        return _accountHolder;
    }

    private void SetAccountHolder(string accountId, Bank bank, AccountHolder _accountHolder)
    {
        // Find the account with the given accountId
        var account = bank.Accounts.Find(a => a.AccountId == accountId);

        if (account != null)
        {
            List<Account> allAccounts = bank.Accounts; 
            List<Transaction> transactions = new List<Transaction>(); 
            _accountHolder = new AccountHolder(account, allAccounts, transactions, bank);
            Console.WriteLine($"Welcome, {accountId}!");
            AccountHolderOperations(_accountHolder,accountId);
        }
        else
        {
            Console.WriteLine("Account not found. Returning to main menu.");
            _accountHolder = null;
            return;
        }
    }
    public void BankStaffOperations()
     {
        while (true)
        {
            Console.WriteLine("\n--- Bank Staff Menu ---");
            Console.WriteLine("1. Create New Account");
            Console.WriteLine("2. View All Accounts");
            Console.WriteLine("3. Add New Currency");
            Console.WriteLine("4. Delete Account");
            Console.WriteLine("5. Update Account");
            Console.WriteLine("6. View Transaction History");
            Console.WriteLine("7. Revert a Transaction");
            Console.WriteLine("8. Back to Main Menu");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _bankStaff.CreateAccount();
                    return;
                case "2":
                    _bankStaff.ViewAllAccounts();
                    return;
                case "3":
                    _bankStaff.AddCurrency();
                    return;
                case "4":
                    _bankStaff.DeleteAccount();
                    return;
                case "5":
                    _bankStaff.UpdateAccount();
                    return;
                case "6":
                    _bankStaff.ViewTransactionHistory();
                    return;
                case "7":
                    _bankStaff.RevertTransaction();
                    return;
                case "8":
                    Console.WriteLine("Returning to main menu.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void AccountHolderOperations(AccountHolder _accountHolder,string accountId)
    {
        if (_accountHolder == null)
        {
            Console.WriteLine("No account holder is logged in. Returning to main menu.");
            return;
        }
        while (true)
        {
            Console.WriteLine("\nAccount Holder Operations:");
            Console.WriteLine("1. View Balance");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. View Transaction History");
            Console.WriteLine("5. Tranfer Funds");
            Console.WriteLine("6. Return to Main Menu");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _accountHolder.ViewBalance(accountId);
                    break;
                case "2":
                    _accountHolder.DepositMoney(accountId);
                    break;
                case "3":
                    _accountHolder.WithdrawMoney(accountId);
                    break;
                case "4":
                    _accountHolder.ViewTransactionHistory(accountId);
                    break;
                case "5":
                    _accountHolder.TransferFunds();
                    return;
                case "6":
                    Console.WriteLine("Returning to main menu.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
