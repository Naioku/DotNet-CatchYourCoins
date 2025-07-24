using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCategory(AppDbContext dbContext) : IRepositoryCategory
{
    public async Task CreateCategoryAsync(Category category) => await dbContext.Categories.AddAsync(category);
    public async Task<Category?> GetCategoryByIdAsync(int id) => await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
}