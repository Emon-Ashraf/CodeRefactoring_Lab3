using System;
using System.Collections.Generic;

namespace PersonalFinanceManagement
{
    public class Currency
    {
        public string Code { get; }
        public string Symbol { get; }

        private Currency(string code, string symbol)
        {
            Code = code;
            Symbol = symbol;
        }

        public override string ToString() => $"{Code}";

        // Static instances to avoid duplication
        public static readonly Currency USD = new Currency("USD", "$");
        public static readonly Currency EUR = new Currency("EUR", "€");
        public static readonly Currency RUB = new Currency("RUB", "₽");

        public static IEnumerable<Currency> All()
        {
            yield return USD;
            yield return EUR;
            yield return RUB;
        }

        public static Currency FromCode(string code)
        {
            foreach (var currency in All())
            {
                if (currency.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
                    return currency;
            }
            throw new ArgumentException("Unsupported currency code.");
        }

        public static Currency FromIndex(int index)
        {
            var list = new List<Currency>(All());
            if (index >= 0 && index < list.Count)
                return list[index];
            throw new ArgumentOutOfRangeException("Invalid currency selection.");
        }
    }
}
