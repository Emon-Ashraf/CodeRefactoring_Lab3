using PersonalFinanceManagement;

public class IncomeTransaction : Operation
{
    public IncomeType IncomeCategory { get; set; }

    public override string Category => Enum.GetName(typeof(IncomeType), IncomeCategory);
    // Other properties in the IncomeTransaction class...

}
