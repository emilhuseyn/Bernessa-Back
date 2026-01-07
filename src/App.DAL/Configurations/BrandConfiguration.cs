using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brands");
            
            builder.HasKey(b => b.Id);
            
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(b => b.Logo)
                .HasMaxLength(500)
                .IsRequired(false);
            
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
