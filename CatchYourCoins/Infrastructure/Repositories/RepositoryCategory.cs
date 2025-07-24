using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCategory(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryCategory
{
    public async Task CreateCategoryAsync(Category category) => await dbContext.Categories.AddAsync(category);

    public async Task<Category?> GetCategoryByIdAsync(int id) =>
        await dbContext.Categories
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(c => c.Id == id);

    public void DeleteCategory(Category category) => dbContext.Categories.Remove(category);
}