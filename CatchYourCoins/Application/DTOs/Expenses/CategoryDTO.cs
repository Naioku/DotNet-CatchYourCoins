namespace Application.DTOs.Expenses;

public class CategoryDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}