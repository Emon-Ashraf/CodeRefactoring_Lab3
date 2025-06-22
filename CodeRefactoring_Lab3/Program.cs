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
            Console.WriteLine("Welcome to Code Refactoring Lab Edition!");

            while (true)
            {
                Console.WriteLine("\n1. R");
                Console.WriteLine("2. L");
                Console.WriteLine("3. E");
                Console.Write("Choice: ");

                string x = Console.ReadLine();

                if (x == "1") Reg();
                else if (x == "2") LogIn();
                else if (x == "3") return;
                else Console.WriteLine("Nope.");
            }
        }

        private static void Reg()
        {
            Console.Write("\nName? ");
            string a = Console.ReadLine();
            Console.Write("Email? ");
            string b = Console.ReadLine();
            Console.Write("Pass? ");
            string c = Console.ReadLine();

            User u = new User(a, b, c);
            users.Add(u);
            Console.WriteLine("Done!");
        }

        //refactored code login method
        private static void LogIn()
        {
            Console.Write("Email? ");
            string e = Console.ReadLine();
            Console.Write("Pass? ");
            string p = Console.ReadLine();

            if (AuthenticateAndSetUser(e, p))
            {
                Console.WriteLine($"Hi, {activeUser.Name}");
                SelectOrCreateWallet();
                RunUserMenu();
            }
            else
            {
                Console.WriteLine("Wrong...");
            }
        }



        private static bool AuthenticateAndSetUser(string email, string password)
        {
            activeUser = Authenticate(email, password);
            return activeUser != null;
        }

        private static void SelectOrCreateWallet()
        {
            var w = activeUser.GetWallets();
            if (w.Count == 0)
            {
                Console.WriteLine("No wallet? Make one!");
                CW();
            }
            else
            {
                for (int i = 0; i < w.Count; i++) Console.WriteLine($"{i + 1}. {w[i].Name}");
                Console.WriteLine($"{w.Count + 1}. New one?");
                Console.Write("Pick: ");

                int y;
                while (!int.TryParse(Console.ReadLine(), out y) || y < 1 || y > w.Count + 1) Console.WriteLine("Try again.");

                if (y <= w.Count) activeUser.SelectActiveWallet(w[y - 1]);
                else CW();
            }
        }

        private static void RunUserMenu()
        {
            while (true)
            {
                Console.WriteLine("\n1. $+");
                Console.WriteLine("2. $-");
                Console.WriteLine("3. Info");
                Console.WriteLine("4. Stats");
                Console.WriteLine("5. X Wallet");
                Console.WriteLine("6. Bye");
                Console.Write("Pick: ");

                string opt = Console.ReadLine();

                switch (opt)
                {
                    case "1": DoIncome(); break;
                    case "2": DoExpense(); break;
                    case "3": Seewallet(); break;
                    case "4": Stats(); break;
                    case "5": DW(); break;
                    case "6": return;
                    default: Console.WriteLine("Hmm?"); break;
                }
            }
        }

        private static void CW()
        {
            Console.Write("Wallet name: ");
            string n = Console.ReadLine();

            Console.WriteLine("Currency:");
            foreach (Currency c in Enum.GetValues(typeof(Currency)))
            {
                Console.WriteLine($"{(int)c}. {c}");
            }

            Console.Write("Pick number: ");
            int t;
            while (!int.TryParse(Console.ReadLine(), out t) || !Enum.IsDefined(typeof(Currency), t)) Console.WriteLine("Nope again.");

            Currency curr = (Currency)t;

            Console.Write("Amount: ");
            double a;
            while (!double.TryParse(Console.ReadLine(), out a) || a < 0) Console.WriteLine("Hmm again.");

            Wallet w = new Wallet(n, curr);
            activeUser.AddWallet(w);
            w.AddIncome((int)IncomeType.Other, a, "Initial money");
        }

        private static void DoIncome()
        {
            if (activeUser != null)
            {
                Console.Write("Income? ");
                string s = Console.ReadLine();
                double m;
                while (!double.TryParse(s, out m) || m <= 0)
                {
                    Console.WriteLine("Nope.");
                    s = Console.ReadLine();
                }

                Console.WriteLine("What type?");
                foreach (IncomeType i in Enum.GetValues(typeof(IncomeType)))
                {
                    Console.WriteLine($"{(int)i}. {i}");
                }

                int it;
                string sit = Console.ReadLine();
                while (!int.TryParse(sit, out it) || !Enum.IsDefined(typeof(IncomeType), it))
                {
                    Console.WriteLine("Nope again.");
                    sit = Console.ReadLine();
                }

                IncomeType typ = (IncomeType)it;
                Console.Write("Desc: ");
                string d = Console.ReadLine();

                activeUser.ActiveWallet.AddIncome((int)typ, m, d);
                Console.WriteLine("Okay.");
            }
            else Console.WriteLine("Login!");
        }

        private static void DoExpense()
        {
            if (activeUser != null)
            {
                Console.Write("Expense? ");
                string s = Console.ReadLine();
                double m;
                while (!double.TryParse(s, out m) || m <= 0)
                {
                    Console.WriteLine("Nope.");
                    s = Console.ReadLine();
                }

                Console.WriteLine("What type?");
                foreach (ExpenseType i in Enum.GetValues(typeof(ExpenseType)))
                {
                    Console.WriteLine($"{(int)i}. {i}");
                }

                int it;
                string sit = Console.ReadLine();
                while (!int.TryParse(sit, out it) || !Enum.IsDefined(typeof(ExpenseType), it))
                {
                    Console.WriteLine("Nope again.");
                    sit = Console.ReadLine();
                }

                ExpenseType typ = (ExpenseType)it;
                Console.Write("Desc: ");
                string d = Console.ReadLine();

                activeUser.ActiveWallet.AddExpense((int)typ, m, d);
                Console.WriteLine("Okay.");
            }
            else Console.WriteLine("Login!");
        }

        private static void Seewallet()
        {
            if (activeUser != null)
            {
                var w = activeUser.GetWallets();
                for (int i = 0; i < w.Count; i++) Console.WriteLine($"{i + 1}. {w[i].Name}");

                Console.Write("Pick one: ");
                int idx;
                while (!int.TryParse(Console.ReadLine(), out idx) || idx < 1 || idx > w.Count)
                {
                    Console.WriteLine("Nope.");
                }

                Wallet sel = w[idx - 1];
                Console.WriteLine(sel.ViewWalletDetails());
            }
        }

        private static void Stats()
        {
            if (activeUser != null)
            {
                Console.Write("Start date (yyyy-MM-dd): ");
                DateTime a;
                while (!DateTime.TryParse(Console.ReadLine(), out a))
                {
                    Console.WriteLine("Try again.");
                }

                Console.Write("End date (yyyy-MM-dd): ");
                DateTime b;
                while (!DateTime.TryParse(Console.ReadLine(), out b))
                {
                    Console.WriteLine("Try again.");
                }

                Console.WriteLine(activeUser.GetStatistics(a, b));
            }
        }

        private static void DW()
        {
            if (activeUser != null)
            {
                var w = activeUser.GetWallets();
                for (int i = 0; i < w.Count; i++) Console.WriteLine($"{i + 1}. {w[i].Name}");

                Console.Write("Pick one to delete: ");
                int idx;
                while (!int.TryParse(Console.ReadLine(), out idx) || idx < 1 || idx > w.Count)
                {
                    Console.WriteLine("Nope.");
                }

                activeUser.RemoveWallet(w[idx - 1]);
            }
        }

        private static User Authenticate(string email, string password)
        {
            return users.Find(x => x.Email == email && x.Authenticate(password));
        }



        //lab2
        public static User TryAuthenticate(string email, string password)
        {
            return Storage.Authenticate(email, password);
        }

    }
}
