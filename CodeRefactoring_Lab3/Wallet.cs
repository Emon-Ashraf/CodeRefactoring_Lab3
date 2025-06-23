using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManagement
{
    public enum IncomeType
    {
        Salary,
        Scholarship,
        Gift,
        Other
    }

    public enum ExpenseType
    {
        Food,
        Restaurants,
        Medicine,
        Sport,
        Taxi,
        Rent,
        Investments,
        Clothes,
        Fun,
        Other
    }

   

    public class Wallet
    {
        public string Name { get; set; }
        private List<Operation> operations;
        public Currency Currency { get; private set; }

        public Wallet(string name, Currency currency, double startingAmount = 0)
        {
            Name = name;
            Currency = currency;
            operations = new List<Operation>();

            if (startingAmount > 0)
            {
                AddIncome(IncomeType.Other, new Money(startingAmount, currency), "Initial balance");
            }
        }

        public void AddOperation(Operation operation)
        {
            operations.Add(operation);
        }

        public List<Operation> GetOperationsByDateRange(DateTime fromDate, DateTime toDate)
        {
            return operations.FindAll(op => op.DateTime >= fromDate && op.DateTime <= toDate);
        }

        public void AddIncome(IncomeType incomeType, Money money, string text)
        {
            if (money.Amount <= 0)
            {
                throw new ArgumentException("Income amount must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Description cannot be empty.");
            }

            IncomeTransaction it = IncomeTransaction.Create(incomeType, money, text);


        }

        public void AddExpense(ExpenseType expenseType, Money money, string text)
        {
            if (money.Amount <= 0)
            {
                throw new ArgumentException("Expense amount must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Description cannot be empty.");
            }

            ExpenseTransaction et = ExpenseTransaction.Create(expenseType, money, text);


        }

        public string ViewWalletDetails()
        {
            return $"Wallet: {Name}\nCurrency: {Currency}\nOperations Count: {operations.Count}";
        }

        public double CalculateTotalIncome()
        {
            return operations.OfType<IncomeTransaction>().Sum(income => income.Money.Amount);
        }

        public double CalculateTotalExpense()
        {
            return operations.OfType<ExpenseTransaction>().Sum(expense => expense.Money.Amount);
        }

        public double TotalBalance => CalculateTotalIncome() - CalculateTotalExpense();

        public string GetStatistics(DateTime startDate, DateTime endDate)
        {
            var operationsWithinRange = operations
                .Where(op => op.DateTime >= startDate && op.DateTime <= endDate)
                .ToList();

            double totalIncome = operationsWithinRange
                .OfType<IncomeTransaction>()
                .Sum(income => income.Amount);

            double totalExpense = operationsWithinRange
                .OfType<ExpenseTransaction>()
                .Sum(expense => expense.Amount);

            int incomeCount = operationsWithinRange.OfType<IncomeTransaction>().Count();
            double averageIncome = incomeCount > 0 ? totalIncome / incomeCount : 0;

            double highestExpense = operationsWithinRange.OfType<ExpenseTransaction>().Any()
                ? operationsWithinRange.OfType<ExpenseTransaction>().Max(expense => expense.Amount)
                : 0;

            string statisticsReport = $"Statistics for {Name} from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:\n";
            statisticsReport += $"Total Income: {totalIncome}\n";
            statisticsReport += $"Total Expense: {totalExpense}\n";
            statisticsReport += $"Average Income: {averageIncome}\n";
            statisticsReport += $"Highest Expense: {highestExpense}\n";

            return statisticsReport;
        }
    }
}
