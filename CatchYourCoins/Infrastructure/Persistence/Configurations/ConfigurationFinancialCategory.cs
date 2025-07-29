using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationFinancialCategory<TCategory> : IEntityTypeConfiguration<TCategory>
    where TCategory : FinancialCategory
{
    public virtual void Configure(EntityTypeBuilder<TCategory> builder)
    {
        ConfigureMainProperties(builder);
        ConfigureRelationships(builder);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getutcdate()")
            .IsRequired();
    }

    private static void ConfigureMainProperties(EntityTypeBuilder<TCategory> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Limit)
            .HasColumnType("decimal(18, 2)");
    }

    private static void ConfigureRelationships(EntityTypeBuilder<TCategory> builder)
    {
        builder
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}