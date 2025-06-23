namespace PersonalFinanceManagement
{
    public class Money
    {
        public double Amount { get; }
        public Currency Currency { get; }


        public Money(double amount, Currency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Money amount cannot be negative.");

            Amount = amount;
            Currency = currency;
        }

        public override string ToString()
        {
            return $"{Amount:F2} {Currency}";
        }

        // Optional enhancements:
        public static Money Zero(Currency currency) => new Money(0, currency);

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add money of different currencies.");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot subtract money of different currencies.");

            double newAmount = Amount - other.Amount;
            if (newAmount < 0) throw new InvalidOperationException("Resulting money cannot be negative.");
            return new Money(newAmount, Currency);
        }
    }
}
