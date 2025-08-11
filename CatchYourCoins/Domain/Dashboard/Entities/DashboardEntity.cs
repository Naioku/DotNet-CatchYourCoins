using Domain.IdentityEntities;
using Domain.Interfaces.Repositories;

namespace Domain.Dashboard.Entities;

public abstract class DashboardEntity : IEntity, IAutorizable
{
    public int Id { get; init; }
    public required Guid UserId { get; init; }
    public AppUser User { get; init; }
}