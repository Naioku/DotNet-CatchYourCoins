using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryCategory(AppDbContext dbContext) : IRepositoryCategory
{
    public async Task CreateCategoryAsync(Category category) => await dbContext.Categories.AddAsync(category);
}