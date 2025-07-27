using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    public DbSet<Domain.Dashboard.Entities.CategoryExpenses> CategoriesExpenses { get; set; }
    public DbSet<Domain.Dashboard.Entities.PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Domain.Dashboard.Entities.Expense> Expenses { get; set; }
    public DbSet<Domain.Dashboard.Entities.CategoryIncomes> CategoriesIncomes { get; set; }
    public DbSet<Domain.Dashboard.Entities.Income> Incomes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}