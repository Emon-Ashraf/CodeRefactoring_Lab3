using System;

namespace PersonalFinanceManagement
{
    public abstract class Operation
    {
        public DateTime DateTime { get; set; }
        public double Amount { get; set; }
        public abstract string Category { get; }
        public string Description { get; set; }
        public Money Money { get; set; }
    }
}
