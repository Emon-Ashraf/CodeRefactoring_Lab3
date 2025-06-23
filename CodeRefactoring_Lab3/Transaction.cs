using System;

namespace PersonalFinanceManagement
{
    public class Transaction : Operation
    {
        public TransactionKind Kind { get; private set; }
        private string _category;
        public override string Category => _category;

        private Transaction() { }                       // force use of factories

        public static Transaction CreateIncome(IncomeType cat, Money money, string desc)
        {
            var t = new Transaction { Kind = TransactionKind.Income, _category = cat.ToString() };
            t.Validate(desc, money);
            return t;
        }

        public static Transaction CreateExpense(ExpenseType cat, Money money, string desc)
        {
            var t = new Transaction { Kind = TransactionKind.Expense, _category = cat.ToString() };
            t.Validate(desc, money);
            return t;
        }
    }
}
