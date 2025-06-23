using PersonalFinanceManagement;

public abstract class Operation
{
    public DateTime DateTime { get; protected set; }
    public double Amount { get; protected set; }
    public abstract string Category { get; }
    public string Description { get; protected set; }
    public Money Money { get; protected set; }

    protected void Validate(string description, Money money)
    {
        if (money == null || money.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.");

        Description = description;
        Money = money;
        Amount = money.Amount;
        DateTime = DateTime.Now;
    }
}
