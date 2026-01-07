using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class ContactSettingConfiguration : IEntityTypeConfiguration<ContactSetting>
    {
        public void Configure(EntityTypeBuilder<ContactSetting> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(500);
            
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(c => c.WhatsApp)
                .HasMaxLength(50)
                .IsRequired(false);
            
            builder.Property(c => c.Instagram)
                .HasMaxLength(200)
                .IsRequired(false);
            
            builder.Property(c => c.WorkingHoursWeekdays)
                .HasMaxLength(100)
                .IsRequired(false);
            
            builder.Property(c => c.WorkingHoursSaturday)
                .HasMaxLength(100)
                .IsRequired(false);
            
            builder.Property(c => c.WorkingHoursSunday)
                .HasMaxLength(100)
                .IsRequired(false);
            
            builder.Property(c => c.SupportDescription)
                .HasMaxLength(1000)
                .IsRequired(false);
            
            builder.Property(c => c.ContactImage)
                .HasMaxLength(500)
                .IsRequired(false);
            
            builder.Property(c => c.Latitude)
                .HasMaxLength(50)
                .IsRequired(false);
            
            builder.Property(c => c.Longitude)
                .HasMaxLength(50)
                .IsRequired(false);
            
            builder.Property(c => c.FacebookUrl)
                .HasMaxLength(500)
                .IsRequired(false);
            
            builder.Property(c => c.TwitterUrl)
                .HasMaxLength(500)
                .IsRequired(false);
            
            builder.Property(c => c.LinkedInUrl)
                .HasMaxLength(500)
                .IsRequired(false);
            
            builder.Property(c => c.YouTubeUrl)
                .HasMaxLength(500)
                .IsRequired(false);
            
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
