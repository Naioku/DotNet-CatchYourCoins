namespace Application.Dashboard.DTOs.OutputDTOs;

public abstract class OutputDTOFinancialCategory : OutputDTOBase
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}