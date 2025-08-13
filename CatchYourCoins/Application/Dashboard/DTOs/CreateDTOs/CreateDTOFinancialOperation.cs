namespace Application.Dashboard.DTOs.CreateDTOs;

public abstract class CreateDTOFinancialOperation : IInputDTODashboardEntity
{
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public int? CategoryId { get; init; }
}