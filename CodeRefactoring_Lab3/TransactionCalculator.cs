using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManagement
{
    public static class TransactionCalculator
    {
        public static double CalculateTotalIncome(List<Operation> operations)
        {
            return operations
                .OfType<Transaction>()
                .Where(t => t.Kind == TransactionKind.Income)
                .Sum(t => t.Money.Amount);
        }

        public static double CalculateTotalExpense(List<Operation> operations)
        {
            return operations
                .OfType<Transaction>()
                .Where(t => t.Kind == TransactionKind.Expense)
                .Sum(t => t.Money.Amount);
        }

        public static double TotalBalance(List<Operation> operations)
        {
            return CalculateTotalIncome(operations) - CalculateTotalExpense(operations);
        }
    }
}
