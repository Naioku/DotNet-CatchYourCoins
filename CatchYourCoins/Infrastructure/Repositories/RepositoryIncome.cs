using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryIncome(AppDbContext dbContext, IServiceCurrentUser serviceCurrentUser) : IRepositoryIncome
{
    public async Task CreateAsync(Income expense) => await dbContext.Incomes.AddAsync(expense);

    public async Task<Income?> GetByIdAsync(int id) =>
        await dbContext.Incomes
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(e => e.Id == id);
    
    public Task<List<Income>> GetAllAsync() =>
        dbContext.Incomes
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .ToListAsync();

    public void Delete(Income expense) => dbContext.Incomes.Remove(expense);
}