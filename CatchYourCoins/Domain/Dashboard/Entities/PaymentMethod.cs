using Domain.IdentityEntities;

namespace Domain.Dashboard.Entities;

public class PaymentMethod
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
    public required Guid UserId { get; init; }
    public required AppUser User { get; init; }
}