using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("orderitems");
            
            builder.HasKey(oi => oi.Id);
            
            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(oi => oi.ProductBrand)
                .HasMaxLength(100);
            
            builder.Property(oi => oi.ProductVolume)
                .HasMaxLength(50);
            
            builder.Property(oi => oi.ProductImage)
                .HasMaxLength(500);
            
            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");
            
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasQueryFilter(oi => !oi.IsDeleted);
        }
    }
}
