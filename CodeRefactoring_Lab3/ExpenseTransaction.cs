using PersonalFinanceManagement;

public class ExpenseTransaction : Operation
{
    public ExpenseType ExpenseCategory { get; set; }

    public override string Category => Enum.GetName(typeof(ExpenseType), ExpenseCategory);

    // Other properties in the ExpenseTransaction class...
}
