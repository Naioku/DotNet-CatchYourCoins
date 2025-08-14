namespace Domain.Dashboard.Entities;

public abstract class FinancialCategory : DashboardEntity
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }

    public DateTime CreatedAt { get; init; }

    protected bool Equals(FinancialCategory other) =>
        base.Equals(other) &&
        Name == other.Name &&
        Limit == other.Limit &&
        CreatedAt.Equals(other.CreatedAt);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FinancialCategory)obj);
    }

    public override int GetHashCode() => HashCode.Combine(
        base.GetHashCode(),
        Name,
        Limit,
        CreatedAt
    );
}