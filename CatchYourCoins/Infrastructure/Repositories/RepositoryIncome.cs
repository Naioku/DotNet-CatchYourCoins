using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryIncome(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser)
    : RepositoryCRUD<Income>(dbContext, serviceCurrentUser), IRepositoryIncome;