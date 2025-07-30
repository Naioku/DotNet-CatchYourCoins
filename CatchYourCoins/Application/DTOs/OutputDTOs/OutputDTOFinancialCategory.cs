namespace Application.DTOs.OutputDTOs;

public abstract class OutputDTOFinancialCategory
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}