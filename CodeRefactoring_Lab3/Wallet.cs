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

        public Wallet(string name, Currency currency, double initialAmount = 0)
        {
            Name = name;
            Currency = currency;
            operations = new List<Operation>();

            if (initialAmount > 0)
            {
                var initialMoney = new Money(initialAmount, currency);
                AddIncome(IncomeType.Other, initialMoney, "Initial balance");
            }
        }

        public void AddOperation(Operation operation)
        {
            operations.Add(operation);
        }

        public void AddIncome(IncomeType incomeType, Money amount, string description)
        {
            var incomeTransaction = Transaction.CreateIncome(incomeType, amount, description);
            operations.Add(incomeTransaction);
        }

        public void AddExpense(ExpenseType expenseType, Money amount, string description)
        {
            var expenseTransaction = Transaction.CreateExpense(expenseType, amount, description);
            operations.Add(expenseTransaction);
        }

        public List<Operation> GetOperationsByDateRange(DateTime fromDate, DateTime toDate)
        {
            return operations
                .Where(op => op.DateTime >= fromDate && op.DateTime <= toDate)
                .ToList();
        }

        public double CalculateTotalIncome()
        {
            return TransactionCalculator.CalculateTotalIncome(operations);
        }

        public double CalculateTotalExpense()
        {
            return TransactionCalculator.CalculateTotalExpense(operations);
        }

        public double TotalBalance => TransactionCalculator.TotalBalance(operations);

        public string ViewWalletDetails()
        {
            return WalletReportGenerator.ViewWalletDetails(this, operations);
        }

        public string GetStatistics(DateTime startDate, DateTime endDate)
        {
            return WalletReportGenerator.GetStatistics(Name, operations, startDate, endDate);
        }
    }
}
