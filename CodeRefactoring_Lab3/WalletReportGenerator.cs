using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManagement
{
    public static class WalletReportGenerator
    {
        public static string ViewWalletDetails(Wallet wallet, List<Operation> operations)
        {
            return $"Wallet: {wallet.Name}\nCurrency: {wallet.Currency}\nOperations Count: {operations.Count}";
        }

        public static string GetStatistics(string walletName, List<Operation> operations, DateTime startDate, DateTime endDate)
        {
            var operationsWithinRange = operations
                .Where(op => op.DateTime >= startDate && op.DateTime <= endDate)
                .ToList();

            double totalIncome = operationsWithinRange.OfType<IncomeTransaction>().Sum(i => i.Amount);
            double totalExpense = operationsWithinRange.OfType<ExpenseTransaction>().Sum(e => e.Amount);

            int incomeCount = operationsWithinRange.OfType<IncomeTransaction>().Count();
            double averageIncome = incomeCount > 0 ? totalIncome / incomeCount : 0;

            double highestExpense = operationsWithinRange.OfType<ExpenseTransaction>().Any()
                ? operationsWithinRange.OfType<ExpenseTransaction>().Max(e => e.Amount)
                : 0;

            return $"Statistics for {walletName} from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:\n" +
                   $"Total Income: {totalIncome}\n" +
                   $"Total Expense: {totalExpense}\n" +
                   $"Average Income: {averageIncome}\n" +
                   $"Highest Expense: {highestExpense}";
        }
    }
}
