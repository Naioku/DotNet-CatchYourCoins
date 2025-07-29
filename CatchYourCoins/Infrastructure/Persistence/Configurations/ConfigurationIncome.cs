using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationIncome : ConfigurationFinancialOperation<Income, CategoryIncomes>
{
    public override void Configure(EntityTypeBuilder<Income> builder)
    {
        base.Configure(builder);
        builder.ToTable("Incomes");
    }
}