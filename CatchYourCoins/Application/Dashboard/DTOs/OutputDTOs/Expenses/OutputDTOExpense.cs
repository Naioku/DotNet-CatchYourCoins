namespace Application.Dashboard.DTOs.OutputDTOs.Expenses;

public class OutputDTOExpense : OutputDTOFinancialOperation
{
    public string? PaymentMethod { get; init; }
}