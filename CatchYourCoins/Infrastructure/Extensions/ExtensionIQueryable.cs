using Domain.Interfaces.Repositories;

namespace Infrastructure.Extensions;

public static class ExtensionIQueryable
{
    public static IQueryable<T> WhereAuthorized<T>(this IQueryable<T> query, Guid loggedInUserId) where T : IAutorizable =>
        query.Where(x => x.UserId == loggedInUserId);
}