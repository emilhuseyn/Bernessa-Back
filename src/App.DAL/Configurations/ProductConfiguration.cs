using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(p => p.Brand)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(p => p.OriginalPrice)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(p => p.Volume)
                .HasMaxLength(50);
            
            builder.Property(p => p.Type)
                .HasMaxLength(100);
            
            builder.Property(p => p.Description)
                .HasMaxLength(2000);
            
            builder.Property(p => p.Images)
                .HasColumnType("text");
            
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
