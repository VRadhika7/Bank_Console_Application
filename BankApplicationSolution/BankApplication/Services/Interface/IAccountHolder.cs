
using BankApplication.Models;

namespace BankApplication.Services.Interface
{
    public interface IAccountHolder
    {
        public void TransferFunds();
        public void WithdrawMoney(string AccountId);
        public void DepositMoney(string AccountId);
        public void ViewBalance(string AccountId);
    }
}
