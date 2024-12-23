using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankApplication.Services.Interface
{
    public interface IBankStaff
    {
        public void CreateAccount();
        public void ViewAllAccounts();
        public void UpdateAccount();
        public void DeleteAccount();
        public void AddCurrency();
        public void ViewTransactionHistory();
        public void LogTransaction(string accountId, string type, decimal amount);
        public void RevertTransaction();
    }
}
