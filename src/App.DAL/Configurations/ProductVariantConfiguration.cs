using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("productvariants");
            
            builder.HasKey(v => v.Id);
            
            builder.Property(v => v.Volume)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(v => v.Price)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(v => v.OriginalPrice)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(v => v.IsActive)
                .HasDefaultValue(true);
            
            builder.HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
