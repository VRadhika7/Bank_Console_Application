using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Services
{
    internal class CommonService
    {
        private readonly Account _account;
        private readonly List<Account> _allAccounts;
        private readonly List<Transaction> _transactions;
        private readonly Bank _bank;

        public CommonService(Account account, List<Account> allAccounts, List<Transaction> transactions, Bank bank)
        {
            _account = account;
            _allAccounts = allAccounts;
            _bank = bank;
            _transactions = transactions ?? new List<Transaction>();
        }
        public bool Login()
        {
            Console.WriteLine("\n--- Login ---");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var account = _allAccounts.FirstOrDefault(a => a.Username == username);

            if (account != null && account.Password == password)
            {
                Console.WriteLine("Login successful!");
                return true;
            }

            Console.WriteLine("Invalid username or password.");
            return false;
        }

    }
}
