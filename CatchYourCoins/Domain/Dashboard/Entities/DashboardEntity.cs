using Domain.IdentityEntities;
using Domain.Interfaces.Repositories;

namespace Domain.Dashboard.Entities;

public abstract class DashboardEntity : IEntity, IAutorizable
{
    public int Id { get; init; }
    public required Guid UserId { get; init; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global - init accesor needed by EF.
    public AppUser? User { get; init; }

    protected bool Equals(DashboardEntity other) =>
        Id == other.Id &&
        UserId.Equals(other.UserId) &&
        Equals(User, other.User);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DashboardEntity)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Id, UserId, User);
}