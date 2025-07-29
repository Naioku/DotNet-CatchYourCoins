using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryCategoryIncomes(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser)
    : RepositoryCRUD<CategoryIncomes>(dbContext, serviceCurrentUser), IRepositoryCategoryIncomes;