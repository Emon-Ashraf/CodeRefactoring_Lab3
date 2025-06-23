using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManagement
{
    public static class TransactionCalculator
    {
        public static double CalculateTotalIncome(List<Operation> operations)
        {
            return operations.OfType<IncomeTransaction>().Sum(op => op.Money.Amount);
        }

        public static double CalculateTotalExpense(List<Operation> operations)
        {
            return operations.OfType<ExpenseTransaction>().Sum(op => op.Money.Amount);
        }

        public static double TotalBalance(List<Operation> operations)
        {
            return CalculateTotalIncome(operations) - CalculateTotalExpense(operations);
        }
    }
}
