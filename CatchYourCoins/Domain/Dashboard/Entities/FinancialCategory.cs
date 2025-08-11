namespace Domain.Dashboard.Entities;

public abstract class FinancialCategory : DashboardEntity
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
    
    public DateTime CreatedAt { get; init; }
}