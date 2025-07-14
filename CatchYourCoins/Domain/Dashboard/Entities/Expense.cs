using Domain.IdentityEntities;

namespace Domain.Dashboard.Entities;

public class Expense
{
    public int Id { get; init; }
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    
    public required Guid UserId { get; init; }
    public AppUser User { get; init; }
    public int? CategoryId { get; init; }
    public Category? Category { get; init; }
    public int? PaymentMethodId { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
    
    public DateTime CreatedAt { get; init; }
}