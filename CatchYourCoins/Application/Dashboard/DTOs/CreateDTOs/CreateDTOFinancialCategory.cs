namespace Application.Dashboard.DTOs.CreateDTOs;

public abstract class CreateDTOFinancialCategory : IInputDTODashboardEntity
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}