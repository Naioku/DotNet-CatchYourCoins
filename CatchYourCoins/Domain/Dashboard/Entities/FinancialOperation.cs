namespace Domain.Dashboard.Entities;

public abstract class FinancialOperation<TCategory> : DashboardEntity
    where TCategory : FinancialCategory
{
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public int? CategoryId { get; init; }
    public TCategory? Category { get; init; }
    
    public DateTime CreatedAt { get; init; }
}