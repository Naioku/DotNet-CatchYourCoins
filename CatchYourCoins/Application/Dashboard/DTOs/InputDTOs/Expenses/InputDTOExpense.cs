namespace Application.Dashboard.DTOs.InputDTOs.Expenses;

public class InputDTOExpense : InputDTOFinancialOperation
{
    public int? PaymentMethodId { get; init; }
}