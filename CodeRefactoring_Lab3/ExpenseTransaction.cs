using System;

namespace PersonalFinanceManagement
{
    public class ExpenseTransaction : Operation
    {
        public ExpenseType ExpenseCategory { get; private set; }
        public override string Category => Enum.GetName(typeof(ExpenseType), ExpenseCategory);

        private ExpenseTransaction() { }

        public static ExpenseTransaction Create(ExpenseType type, Money money, string description)
        {
            var transaction = new ExpenseTransaction
            {
                ExpenseCategory = type
            };
            transaction.Validate(description, money);
            return transaction;
        }
    }
}
