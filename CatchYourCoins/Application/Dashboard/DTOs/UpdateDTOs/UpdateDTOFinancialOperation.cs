namespace Application.Dashboard.DTOs.UpdateDTOs;

public abstract class UpdateDTOFinancialOperation
{
    public required int Id { get; init; }
    public Optional<decimal?> Amount { get; init; } = new();
    public Optional<DateTime?> Date { get; init; } = new();
    public Optional<string?> Description { get; init; } = new();
    public Optional<int?> CategoryId { get; init; } = new();
}