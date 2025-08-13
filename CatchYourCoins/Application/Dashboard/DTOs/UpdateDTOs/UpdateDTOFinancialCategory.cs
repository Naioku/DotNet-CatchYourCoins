namespace Application.Dashboard.DTOs.UpdateDTOs;

public abstract class UpdateDTOFinancialCategory : IInputDTODashboardEntity
{
    public required int Id { get; init; }
    public Optional<string> Name { get; init; } = new();
    public Optional<decimal?> Limit { get; init; } = new();
}