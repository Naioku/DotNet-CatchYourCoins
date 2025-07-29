using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationExpense : ConfigurationFinancialOperation<Expense, CategoryExpenses>
{
    public override void Configure(EntityTypeBuilder<Expense> builder)
    {
        base.Configure(builder);
        builder.ToTable("Expenses");
    }
}