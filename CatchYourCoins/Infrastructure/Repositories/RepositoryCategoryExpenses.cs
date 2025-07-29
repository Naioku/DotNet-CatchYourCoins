using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryCategoryExpenses(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser)
    : RepositoryCRUD<CategoryExpenses>(dbContext, serviceCurrentUser), IRepositoryCategoryExpenses;