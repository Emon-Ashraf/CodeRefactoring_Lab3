using System;

namespace PersonalFinanceManagement
{
    public class IncomeTransaction : Operation
    {
        public IncomeType IncomeCategory { get; private set; }
        public override string Category => Enum.GetName(typeof(IncomeType), IncomeCategory);

        private IncomeTransaction() { }

        public static IncomeTransaction Create(IncomeType type, Money money, string description)
        {
            var transaction = new IncomeTransaction
            {
                IncomeCategory = type
            };
            transaction.Validate(description, money);
            return transaction;
        }
    }
}
