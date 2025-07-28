namespace Domain.Dashboard.Entities;

public class Expense : FinancialOperation<CategoryExpenses>
{
    public int? PaymentMethodId { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
}