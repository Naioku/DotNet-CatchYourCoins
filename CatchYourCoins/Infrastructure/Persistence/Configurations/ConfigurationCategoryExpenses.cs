using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationCategoryExpenses : IEntityTypeConfiguration<CategoryExpenses>
{
    public void Configure(EntityTypeBuilder<CategoryExpenses> builder)
    {
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<CategoryExpenses> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Limit)
            .HasColumnType("decimal(18, 2)");
    }

    private static void ConfigureRelationships(EntityTypeBuilder<CategoryExpenses> builder)
    {
        builder
            .HasOne(c => c.User)
            .WithMany(u => u.CategoriesExpenses)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}