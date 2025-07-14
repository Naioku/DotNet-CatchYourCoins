using Domain.Dashboard.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryCategory
{
    Task CreateCategoryAsync(Category category);
}