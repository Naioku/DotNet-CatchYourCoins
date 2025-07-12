using Domain.Dashboard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfigurationPaymentMethod : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }
    
    private static void ConfigureProperties(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Limit)
            .HasColumnType("decimal(18, 2)");
    }

    private static void ConfigureRelationships(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder
            .HasOne(c => c.User)
            .WithMany(u => u.PaymentMethods)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}