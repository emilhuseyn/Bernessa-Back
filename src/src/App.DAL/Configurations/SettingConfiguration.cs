using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("settings");
            
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.StoreName)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(s => s.Address)
                .HasMaxLength(500);
            
            builder.Property(s => s.ShippingCost)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(s => s.MinOrderAmount)
                .HasColumnType("decimal(18,2)");
            
            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
