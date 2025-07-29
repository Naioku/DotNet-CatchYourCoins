using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationCategoryIncomes : ConfigurationFinancialCategory<CategoryIncomes>
{
    public override void Configure(EntityTypeBuilder<CategoryIncomes> builder)
    {
        base.Configure(builder);
        builder.ToTable("CategoriesIncomes");
    }
}