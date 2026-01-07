using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            
            builder.HasKey(o => o.Id);
            
            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();
            
            builder.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(o => o.CustomerEmail)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(o => o.CustomerPhone)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(500);
            
            builder.Property(o => o.CustomerNote)
                .HasMaxLength(1000);
            
            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(o => o.Tax)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(o => o.Discount)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(o => o.Total)
                .HasColumnType("decimal(18,2)");
            
            builder.HasQueryFilter(o => !o.IsDeleted);
        }
    }
}
