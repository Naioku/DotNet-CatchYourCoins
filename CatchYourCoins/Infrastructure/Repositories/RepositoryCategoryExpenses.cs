using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCategoryExpenses(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryCategoryExpenses
{
    public async Task CreateAsync(CategoryExpenses category) => await dbContext.CategoriesExpenses.AddAsync(category);

    public async Task<CategoryExpenses?> GetByIdAsync(int id) =>
        await dbContext.CategoriesExpenses
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<CategoryExpenses>> GetAllAsync() =>
        dbContext.CategoriesExpenses
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .ToListAsync();

    public void Delete(CategoryExpenses category) => dbContext.CategoriesExpenses.Remove(category);
}