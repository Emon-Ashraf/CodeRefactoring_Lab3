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

        public Wallet ActiveWallet { get; private set; }

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

        public void AddWallet(Wallet newWallet)
        {
            if (newWallet == null)
                throw new ArgumentNullException(nameof(newWallet));

            wallets.Add(newWallet);
        }

        public void RemoveWallet(string walletName)
        {
            var walletToRemove = wallets.Find(w => w.Name.Equals(walletName, StringComparison.OrdinalIgnoreCase));
            if (walletToRemove != null)
            {
                wallets.Remove(walletToRemove);
            }
        }

        public void RemoveWallet(Wallet wallet)
        {
            if (wallet != null && wallets.Contains(wallet))
            {
                wallets.Remove(wallet);
            }
        }

        public List<Wallet> GetWallets()
        {
            return wallets;
        }

        public Wallet GetWalletByName(string walletName)
        {
            return wallets.Find(w => w.Name.Equals(walletName, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectActiveWallet(Wallet wallet)
        {
            if (wallet == null || !wallets.Contains(wallet))
                throw new InvalidOperationException("Wallet does not exist or is not owned by the user.");

            ActiveWallet = wallet;
        }

        public void AddIncome(IncomeType type, Money amount, string description)
        {
            if (ActiveWallet == null)
            {
                Console.WriteLine("Please select a wallet first.");
                return;
            }

            ActiveWallet.AddIncome(type, amount, description);
        }

        public void AddExpense(ExpenseType type, Money amount, string description)
        {
            if (ActiveWallet == null)
            {
                Console.WriteLine("Please select a wallet first.");
                return;
            }

            ActiveWallet.AddExpense(type, amount, description);
        }

        public string ViewStatistics(DateTime fromDate, DateTime toDate)
        {
            return ActiveWallet != null
                ? ActiveWallet.GetStatistics(fromDate, toDate)
                : "Please create and select a wallet to view statistics.";
        }
    }
}
