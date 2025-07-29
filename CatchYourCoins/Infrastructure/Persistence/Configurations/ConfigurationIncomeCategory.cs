using Domain.Dashboard.Entities.Incomes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationIncomeCategory : ConfigurationFinancialCategory<IncomeCategory>
{
    public override void Configure(EntityTypeBuilder<IncomeCategory> builder)
    {
        base.Configure(builder);
        builder.ToTable("IncomeCategories");
    }
}