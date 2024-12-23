namespace BankApplication.Models
{
    public class Currency
    {
        public string Code { get; }
        public decimal ExchangeRate { get; }

        public Currency(string code, decimal rate)
        {
            Code = code;
            ExchangeRate = rate;
        }
    }
}
