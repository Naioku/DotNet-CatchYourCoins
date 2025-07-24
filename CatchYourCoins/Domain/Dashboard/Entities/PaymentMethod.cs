using Domain.IdentityEntities;
using Domain.Interfaces.Repositories;

namespace Domain.Dashboard.Entities;

public class PaymentMethod : IAutorizable
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
    public required Guid UserId { get; init; }
    public AppUser User { get; init; }
}