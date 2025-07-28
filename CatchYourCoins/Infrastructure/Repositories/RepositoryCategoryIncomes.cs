using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCategoryIncomes(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryCategoryIncomes
{
    public async Task CreateAsync(CategoryIncomes category) => await dbContext.CategoriesIncomes.AddAsync(category);

    public async Task<CategoryIncomes?> GetByIdAsync(int id) =>
        await dbContext.CategoriesIncomes
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<CategoryIncomes>> GetAllAsync() =>
        dbContext.CategoriesIncomes
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .ToListAsync();

    public void Delete(CategoryIncomes category) => dbContext.CategoriesIncomes.Remove(category);
}