namespace Application.Dashboard.DTOs.OutputDTOs;

public abstract class OutputDTOFinancialOperation : OutputDTOBase
{
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
}