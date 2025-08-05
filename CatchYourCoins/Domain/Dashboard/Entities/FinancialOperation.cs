using Domain.IdentityEntities;
using Domain.Interfaces.Repositories;

namespace Domain.Dashboard.Entities;

public abstract class FinancialOperation<TCategory> : IAutorizable, IEntity
    where TCategory : FinancialCategory
{
    public int Id { get; init; }
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    
    public required Guid UserId { get; init; }
    public AppUser User { get; init; }
    public int? CategoryId { get; init; }
    public TCategory? Category { get; init; }
    
    public DateTime CreatedAt { get; init; }
}