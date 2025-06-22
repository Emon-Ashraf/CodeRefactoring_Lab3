using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PersonalFinanceManagement
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        private List<Wallet> wallets;

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            PasswordHash = HashPassword(password);
            wallets = new List<Wallet>();
        }

        public bool Authenticate(string password)
        {
            string hashedPassword = HashPassword(password);
            return PasswordHash.Equals(hashedPassword);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
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

        public void AddIncome(IncomeType selectedIncomeType, double amount, string description)
        {
            if (ActiveWallet != null)
            {
                ActiveWallet.AddIncome((int)selectedIncomeType, amount, description);
            }
            else
            {
                Console.WriteLine("Please create/select a wallet before adding income.");
            }
        }

        public void AddExpense(ExpenseType selectedExpenseType, double amount, string description)
        {
            if (ActiveWallet != null)
            {
                ActiveWallet.AddExpense((int)selectedExpenseType, amount, description);
            }
            else
            {
                Console.WriteLine("Please create/select a wallet before adding expense.");
            }
        }

        public string GetStatistics(DateTime startDate, DateTime endDate)
        {
            if (ActiveWallet != null)
            {
                return ActiveWallet.GetStatistics(startDate, endDate);
            }
            else
            {
                return "Please create/select a wallet to view statistics.";
            }
        }

    }
}
