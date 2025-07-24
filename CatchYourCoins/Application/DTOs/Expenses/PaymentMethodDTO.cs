namespace Application.DTOs.Expenses;

public class PaymentMethodDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}