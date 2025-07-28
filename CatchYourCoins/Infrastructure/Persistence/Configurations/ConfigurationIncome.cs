using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationIncome : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        ConfigureMainProperties(builder);
        ConfigureRelationships(builder);

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getutcdate()")
            .IsRequired();
    }

    private static void ConfigureMainProperties(EntityTypeBuilder<Income> builder)
    {
        builder.Property(e => e.Amount)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(e => e.Date)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(8000);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Income> builder)
    {
        builder
            .HasOne(e => e.User)
            .WithMany(u => u.Incomes)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}