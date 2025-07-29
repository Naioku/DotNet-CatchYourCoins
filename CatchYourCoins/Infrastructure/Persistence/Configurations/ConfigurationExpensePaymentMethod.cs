using Domain.Dashboard.Entities.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationExpensePaymentMethod : ConfigurationFinancialCategory<ExpensePaymentMethod>
{
    public override void Configure(EntityTypeBuilder<ExpensePaymentMethod> builder)
    {
        base.Configure(builder);
        builder.ToTable("ExpensePaymentMethods");
    }
}