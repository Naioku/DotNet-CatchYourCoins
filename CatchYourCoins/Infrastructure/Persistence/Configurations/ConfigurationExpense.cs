using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationExpense : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        ConfigureMainProperties(builder);
        ConfigureRelationships(builder);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getutcdate()")
            .IsRequired();
    }

    private static void ConfigureMainProperties(EntityTypeBuilder<Expense> builder)
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

    private static void ConfigureRelationships(EntityTypeBuilder<Expense> builder)
    {
        builder
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.
            HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.
            HasOne(e => e.PaymentMethod)
            .WithMany()
            .HasForeignKey(e => e.PaymentMethodId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}