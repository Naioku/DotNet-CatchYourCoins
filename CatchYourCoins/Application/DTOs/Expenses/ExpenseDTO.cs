namespace Application.DTOs.Expenses;

public class ExpenseDTO
{
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? PaymentMethod { get; init; }
}