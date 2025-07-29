namespace Domain.Dashboard.Entities.Expenses;

public class Expense : FinancialOperation<ExpenseCategory>
{
    public int? PaymentMethodId { get; init; }
    public ExpensePaymentMethod? PaymentMethod { get; init; }
}