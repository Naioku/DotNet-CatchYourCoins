using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationCategoryIncomes : IEntityTypeConfiguration<CategoryIncomes>
{
    public void Configure(EntityTypeBuilder<CategoryIncomes> builder)
    {
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<CategoryIncomes> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Limit)
            .HasColumnType("decimal(18, 2)");
    }

    private static void ConfigureRelationships(EntityTypeBuilder<CategoryIncomes> builder)
    {
        builder
            .HasOne(c => c.User)
            .WithMany(u => u.CategoriesIncomes)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}