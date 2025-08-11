namespace Application.Dashboard.DTOs.UpdateDTOs.Expenses;

public class UpdateDTOExpense : UpdateDTOFinancialOperation
{
    public Optional<int?> PaymentMethodId { get; init; } = new();
}