namespace Application.Dashboard.DTOs.UpdateDTOs.Expenses;

public class UpdateDTOExpense : UpdateDTOFinancialOperation
{
    public Optional<int?> PaymentMethodId { get; private set; } = new();

    public int? SetPaymentMethodId
    {
        init => PaymentMethodId = new Optional<int?>(value);
    }
}