namespace Application.Dashboard.DTOs.UpdateDTOs;

public abstract class UpdateDTOFinancialOperation : IInputDTODashboardEntity
{
    public required int Id { get; init; }
    public Optional<decimal> Amount { get; private set; } = new();
    public Optional<DateTime> Date { get; private set; } = new();
    public Optional<string?> Description { get; private set; } = new();
    public Optional<int?> CategoryId { get; private set; } = new();
    
    public decimal SetAmount
    {
        init => Amount = new Optional<decimal>(value);
    }
    
    public DateTime SetDate
    {
        init => Date = new Optional<DateTime>(value);
    }
    
    public string? SetDescription
    {
        init => Description = new Optional<string?>(value);
    }
    
    public int? SetCategoryId
    {
        init => CategoryId = new Optional<int?>(value);
    }
}