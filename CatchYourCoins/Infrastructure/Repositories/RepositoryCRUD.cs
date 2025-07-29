using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCRUD<TEntity>(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryCRUD<TEntity>
    where TEntity : class, IAutorizable, IEntity
{
    public async Task CreateAsync(TEntity entity) => await dbContext.Set<TEntity>().AddAsync(entity);

    public async Task<TEntity?> GetByIdAsync(int id) =>
        await dbContext.Set<TEntity>()
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<TEntity>> GetAllAsync() =>
        dbContext.Set<TEntity>()
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .ToListAsync();

    public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
}