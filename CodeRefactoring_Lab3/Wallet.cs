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
            return WalletReportGenerator.ViewWalletDetails(this, operations);
        }



        public double CalculateTotalIncome() => TransactionCalculator.CalculateTotalIncome(operations);
        public double CalculateTotalExpense() => TransactionCalculator.CalculateTotalExpense(operations);
        public double TotalBalance => TransactionCalculator.TotalBalance(operations);



        public string GetStatistics(DateTime startDate, DateTime endDate)
        {
            return WalletReportGenerator.GetStatistics(Name, operations, startDate, endDate);
        }


    }
}
