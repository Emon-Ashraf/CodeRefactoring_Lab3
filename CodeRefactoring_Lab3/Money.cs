using System;

namespace PersonalFinanceManagement
{
    /// <summary>
    /// Represents a value object for handling monetary amounts in a specific currency.
    /// </summary>
    public class Money
    {
        public double Amount { get; }
        public Currency Currency { get; }

        public Money(double amount, Currency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be non-negative.", nameof(amount));

            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            Amount = amount;
        }

        /// <summary>
        /// Returns a formatted string representation of the amount and currency.
        /// </summary>
        public override string ToString()
        {
            return $"{Currency.Symbol}{Amount:F2} {Currency.Code}";
        }

        /// <summary>
        /// Returns a Money object representing zero in the given currency.
        /// </summary>
        public static Money Zero(Currency currency) => new Money(0, currency);

        /// <summary>
        /// Returns a new Money instance by adding another Money instance of the same currency.
        /// </summary>
        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount, Currency);
        }

        /// <summary>
        /// Returns a new Money instance by subtracting another Money instance of the same currency.
        /// </summary>
        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);

            double result = Amount - other.Amount;
            if (result < 0)
                throw new InvalidOperationException("Subtraction would result in a negative balance.");

            return new Money(result, Currency);
        }

        /// <summary>
        /// Ensures that another Money instance has the same currency.
        /// </summary>
        private void EnsureSameCurrency(Money other)
        {
            if (Currency.Code != other.Currency.Code)
                throw new InvalidOperationException("Currency mismatch between amounts.");
        }
    }
}
