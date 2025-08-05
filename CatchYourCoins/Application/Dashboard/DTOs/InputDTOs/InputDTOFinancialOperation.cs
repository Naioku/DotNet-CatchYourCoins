namespace Application.Dashboard.DTOs.InputDTOs;

public class InputDTOFinancialOperation
{
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public int? CategoryId { get; init; }
}