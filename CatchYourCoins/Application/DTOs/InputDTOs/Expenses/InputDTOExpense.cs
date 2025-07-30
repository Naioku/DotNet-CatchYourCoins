namespace Application.DTOs.InputDTOs.Expenses;

public class InputDTOExpense : InputDTOFinancialOperation
{
    public int? PaymentMethodId { get; init; }
}