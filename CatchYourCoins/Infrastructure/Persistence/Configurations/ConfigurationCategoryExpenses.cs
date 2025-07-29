using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationCategoryExpenses : ConfigurationFinancialCategory<CategoryExpenses>
{
    public override void Configure(EntityTypeBuilder<CategoryExpenses> builder)
    {
        base.Configure(builder);
        builder.ToTable("CategoriesExpenses");
    }
}