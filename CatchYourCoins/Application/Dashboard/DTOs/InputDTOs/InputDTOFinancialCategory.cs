namespace Application.Dashboard.DTOs.InputDTOs;

public abstract class InputDTOFinancialCategory
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}