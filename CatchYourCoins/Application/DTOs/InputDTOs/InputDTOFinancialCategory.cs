namespace Application.DTOs.InputDTOs;

public class InputDTOFinancialCategory
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}