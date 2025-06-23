using System;
using System.Collections.Generic;

namespace PersonalFinanceManagement
{
    public class Program
    {
        private static List<User> users = new List<User>();
        private static User activeUser;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Personal Finance Management System!");

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                if (choice == "1") RegisterUser();
                else if (choice == "2") LoginUser();
                else if (choice == "3") return;
                else Console.WriteLine("Invalid option. Please try again.");
            }
        }

        private static void RegisterUser()
        {
            Console.Write("\nEnter your name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your email: ");
            string email = Console.ReadLine();

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            User user = new User(name, email, password);
            users.Add(user);
            Console.WriteLine("Registration successful!");
        }

        private static void LoginUser()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (AuthenticateAndSetUser(email, password))
            {
                Console.WriteLine($"\nWelcome, {activeUser.Name}!");
                SelectOrCreateWallet();
                RunUserMenu();
            }
            else
            {
                Console.WriteLine("Login failed. Incorrect credentials.");
            }
        }

        private static bool AuthenticateAndSetUser(string email, string password)
        {
            activeUser = Authenticate(email, password);
            return activeUser != null;
        }

        private static void SelectOrCreateWallet()
        {
            var wallets = activeUser.GetWallets();
            if (wallets.Count == 0)
            {
                Console.WriteLine("You don't have a wallet yet. Let's create one.");
                CreateWallet();
            }
            else
            {
                Console.WriteLine("Available wallets:");
                for (int i = 0; i < wallets.Count; i++)
                    Console.WriteLine($"{i + 1}. {wallets[i].Name}");

                Console.WriteLine($"{wallets.Count + 1}. Create a new wallet");
                Console.Write("Select a wallet: ");

                int selectedIndex;
                while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > wallets.Count + 1)
                    Console.WriteLine("Invalid input. Try again.");

                if (selectedIndex <= wallets.Count)
                    activeUser.SelectActiveWallet(wallets[selectedIndex - 1]);
                else
                    CreateWallet();
            }
        }

        private static void RunUserMenu()
        {
            while (true)
            {
                Console.WriteLine("\nWallet Menu:");
                Console.WriteLine("1. Add Income");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. View Wallet");
                Console.WriteLine("4. View Statistics");
                Console.WriteLine("5. Delete Wallet");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1": HandleIncome(); break;
                    case "2": HandleExpense(); break;
                    case "3": ViewWalletDetailsMenu(); break;
                    case "4": ShowStatistics(); break;
                    case "5": DeleteWallet(); break;
                    case "6": return;
                    default: Console.WriteLine("Unknown option. Please try again."); break;
                }
            }
        }

        private static void CreateWallet()
        {
            Console.Write("Enter wallet name: ");
            string walletName = Console.ReadLine();

            Console.WriteLine("Select a currency:");
            var currencies = new List<Currency>(Currency.All());
            for (int i = 0; i < currencies.Count; i++)
                Console.WriteLine($"{i + 1}. {currencies[i].Code} ({currencies[i].Symbol})");

            int selectedCurrencyIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedCurrencyIndex) || selectedCurrencyIndex < 1 || selectedCurrencyIndex > currencies.Count)
                Console.WriteLine("Invalid selection. Try again.");

            Currency selectedCurrency = currencies[selectedCurrencyIndex - 1];

            Console.Write("Enter starting amount: ");
            double amount;
            while (!double.TryParse(Console.ReadLine(), out amount) || amount < 0)
                Console.WriteLine("Amount must be a positive number. Try again.");

            Wallet wallet = new Wallet(walletName, selectedCurrency);
            activeUser.AddWallet(wallet);
            Money initial = new Money(amount, selectedCurrency);
            wallet.AddIncome(IncomeType.Other, initial, "Initial balance");
            Console.WriteLine("Wallet created successfully.");
        }

        private static void HandleTransaction<TEnum>(string typeLabel, Func<TEnum, Money, string, string> processFunc) where TEnum : Enum
        {
            if (activeUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }

            double amount = GetValidAmount($"{typeLabel} amount");
            TEnum category = GetValidEnum<TEnum>($"{typeLabel} type");

            Console.Write("Enter a description: ");
            string description = Console.ReadLine();

            var money = new Money(amount, activeUser.ActiveWallet.Currency);
            string result = processFunc(category, money, description);

            Console.WriteLine(result);
        }

        private static void HandleIncome()
        {
            HandleTransaction<IncomeType>("Income", (type, money, description) =>
            {
                activeUser.AddIncome(type, money, description);
                return "Income recorded successfully.";
            });
        }

        private static void HandleExpense()
        {
            HandleTransaction<ExpenseType>("Expense", (type, money, description) =>
            {
                activeUser.AddExpense(type, money, description);
                return "Expense recorded successfully.";
            });
        }

        private static double GetValidAmount(string prompt)
        {
            Console.Write($"{prompt}: ");
            string input = Console.ReadLine();
            double value;
            while (!double.TryParse(input, out value) || value <= 0)
            {
                Console.WriteLine("Invalid amount. Try again.");
                input = Console.ReadLine();
            }
            return value;
        }

        private static TEnum GetValidEnum<TEnum>(string prompt) where TEnum : Enum
        {
            Console.WriteLine($"Select {prompt}:");
            foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
                Console.WriteLine($"{(int)(object)item}. {item}");

            int selection;
            string input = Console.ReadLine();
            while (!int.TryParse(input, out selection) || !Enum.IsDefined(typeof(TEnum), selection))
            {
                Console.WriteLine("Invalid selection. Try again.");
                input = Console.ReadLine();
            }

            return (TEnum)(object)selection;
        }

        private static void ViewWalletDetailsMenu()
        {
            if (activeUser != null)
            {
                var wallets = activeUser.GetWallets();
                for (int i = 0; i < wallets.Count; i++)
                    Console.WriteLine($"{i + 1}. {wallets[i].Name}");

                Console.Write("Select a wallet to view: ");
                int index;
                while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > wallets.Count)
                    Console.WriteLine("Invalid selection. Try again.");

                Wallet selected = wallets[index - 1];
                Console.WriteLine(selected.ViewWalletDetails());
            }
        }

        private static void ShowStatistics()
        {
            if (activeUser != null)
            {
                Console.Write("Enter start date (yyyy-MM-dd): ");
                DateTime startDate;
                while (!DateTime.TryParse(Console.ReadLine(), out startDate))
                    Console.WriteLine("Invalid format. Try again (yyyy-MM-dd).");

                Console.Write("Enter end date (yyyy-MM-dd): ");
                DateTime endDate;
                while (!DateTime.TryParse(Console.ReadLine(), out endDate))
                    Console.WriteLine("Invalid format. Try again (yyyy-MM-dd).");

                Console.WriteLine(activeUser.ViewStatistics(startDate, endDate));
            }
        }

        private static void DeleteWallet()
        {
            if (activeUser != null)
            {
                var wallets = activeUser.GetWallets();
                for (int i = 0; i < wallets.Count; i++)
                    Console.WriteLine($"{i + 1}. {wallets[i].Name}");

                Console.Write("Select a wallet to delete: ");
                int index;
                while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > wallets.Count)
                    Console.WriteLine("Invalid input. Try again.");

                activeUser.RemoveWallet(wallets[index - 1]);
                Console.WriteLine("Wallet deleted.");
            }
        }

        private static User Authenticate(string email, string password)
        {
            return users.Find(user => user.Email == email && user.Authenticate(password));
        }

        public static User TryAuthenticate(string email, string password)
        {
            return Storage.Authenticate(email, password);
        }
    }
}
