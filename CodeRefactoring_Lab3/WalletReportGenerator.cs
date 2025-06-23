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
            var range = operations
                .OfType<Transaction>()
                .Where(op => op.DateTime >= startDate && op.DateTime <= endDate)
                .ToList();

            var incomeTransactions = range.Where(t => t.Kind == TransactionKind.Income).ToList();
            var expenseTransactions = range.Where(t => t.Kind == TransactionKind.Expense).ToList();

            double totalIncome = incomeTransactions.Sum(t => t.Amount);
            double totalExpense = expenseTransactions.Sum(t => t.Amount);
            int incomeCount = incomeTransactions.Count;

            double averageIncome = incomeCount > 0 ? totalIncome / incomeCount : 0;

            double highestExpense = expenseTransactions.Any()
                ? expenseTransactions.Max(e => e.Amount)
                : 0;

            return $"Statistics for {walletName} from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:\n" +
                   $"Total Income: {totalIncome}\n" +
                   $"Total Expense: {totalExpense}\n" +
                   $"Average Income: {averageIncome}\n" +
                   $"Highest Expense: {highestExpense}";
        }


    }
}
