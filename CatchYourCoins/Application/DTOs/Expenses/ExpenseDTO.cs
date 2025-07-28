namespace Application.DTOs.Expenses;

public class ExpenseDTO : FinancialOperationDTO
{
    public string? PaymentMethod { get; init; }
}