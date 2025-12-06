using App.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(u => u.Avatar)
                .IsRequired(false)  // Make it optional/nullable
                .HasMaxLength(500);
            
             
        }
    }
}
