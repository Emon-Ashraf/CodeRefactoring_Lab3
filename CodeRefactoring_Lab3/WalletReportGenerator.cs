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
                .OfType<Transaction>()
                .Where(op => op.DateTime >= startDate && op.DateTime <= endDate)
                .ToList();

            double totalIncome = operationsWithinRange
                .Where(t => t.Kind == TransactionKind.Income)
                .Sum(t => t.Amount);

            double totalExpense = operationsWithinRange
                .Where(t => t.Kind == TransactionKind.Expense)
                .Sum(t => t.Amount);

            int incomeCount = operationsWithinRange
                .Count(t => t.Kind == TransactionKind.Income);

            double averageIncome = incomeCount > 0 ? totalIncome / incomeCount : 0;

            double highestExpense = operationsWithinRange
                .Where(t => t.Kind == TransactionKind.Expense)
                .DefaultIfEmpty()
                .Max(t => t?.Amount ?? 0);

            return $"Statistics for {walletName} from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:\n" +
                   $"Total Income: {totalIncome}\n" +
                   $"Total Expense: {totalExpense}\n" +
                   $"Average Income: {averageIncome}\n" +
                   $"Highest Expense: {highestExpense}";
        }

    }
}
