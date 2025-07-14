using Domain.IdentityEntities;

namespace Domain.Dashboard.Entities;

public class Category
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
    public required Guid UserId { get; init; }
    public AppUser User { get; }
}