using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCRUD<T>(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryCRUD<T>
    where T : class, IAutorizable, IEntity
{
    public async Task CreateAsync(T category) => await dbContext.Set<T>().AddAsync(category);

    public async Task<T?> GetByIdAsync(int id) =>
        await dbContext.Set<T>()
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<T>> GetAllAsync() =>
        dbContext.Set<T>()
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .ToListAsync();

    public void Delete(T category) => dbContext.Set<T>().Remove(category);
}