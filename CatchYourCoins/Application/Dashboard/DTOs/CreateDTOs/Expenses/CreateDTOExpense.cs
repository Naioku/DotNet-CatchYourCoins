namespace Application.Dashboard.DTOs.CreateDTOs.Expenses;

public class CreateDTOExpense : CreateDTOFinancialOperation
{
    public int? PaymentMethodId { get; init; }
}