using System;
using System.Collections.Generic;

namespace PersonalFinanceManagement
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Password Password { get; set; }
        private List<Wallet> wallets;

        public User(string name, string email, string plainTextPassword)
        {
            Name = name;
            Email = email;
            Password = new Password(plainTextPassword);
            wallets = new List<Wallet>();
        }

        public bool Authenticate(string plainTextPassword)
        {
            return Password.Verify(plainTextPassword);
        }

        public void CreateWallet(string walletName, Currency currency)
        {
            Wallet newWallet = new Wallet(walletName, currency);
            wallets.Add(newWallet);
        }

        public void AddWallet(Wallet newWallet)
        {
            wallets.Add(newWallet);
        }

        public void RemoveWallet(string walletName)
        {
            Wallet walletToRemove = wallets.Find(wallet => wallet.Name.Equals(walletName));
            if (walletToRemove != null)
            {
                wallets.Remove(walletToRemove);
            }
        }

        public void RemoveWallet(Wallet wallet)
        {
            if (wallets.Contains(wallet))
            {
                wallets.Remove(wallet);
            }
        }

        public Wallet GetWalletByName(string walletName)
        {
            return wallets.Find(wallet => wallet.Name.Equals(walletName));
        }

        public List<Wallet> GetWallets()
        {
            return wallets;
        }

        public Wallet ActiveWallet { get; private set; }

        public void SelectActiveWallet(Wallet wallet)
        {
            ActiveWallet = wallet;
        }

        public void AddIncome(IncomeType type, Money money, string description)
        {
            ActiveWallet?.AddIncome(type, money, description);
        }

        public void AddExpense(ExpenseType type, Money money, string description)
        {
            ActiveWallet?.AddExpense(type, money, description);
        }

        public string ViewStatistics(DateTime from, DateTime to)
        {
            return ActiveWallet != null
                ? ActiveWallet.GetStatistics(from, to)
                : "Please create/select a wallet to view statistics.";
        }
    }
}
