using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.Configurations
{
    public class ContactSettingTranslationConfiguration : IEntityTypeConfiguration<ContactSettingTranslation>
    {
        public void Configure(EntityTypeBuilder<ContactSettingTranslation> builder)
        {
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);
            
            builder.Property(t => t.SupportDescription)
                .HasMaxLength(1000)
                .IsRequired(false);
            
            builder.Property(t => t.WorkingHoursWeekdays)
                .HasMaxLength(100)
                .IsRequired(false);
            
            builder.Property(t => t.WorkingHoursSaturday)
                .HasMaxLength(100)
                .IsRequired(false);
            
            builder.Property(t => t.WorkingHoursSunday)
                .HasMaxLength(100)
                .IsRequired(false);
            
            builder.HasOne(t => t.ContactSetting)
                .WithMany(c => c.Translations)
                .HasForeignKey(t => t.ContactSettingId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(t => new { t.ContactSettingId, t.LanguageCode })
                .IsUnique();
        }
    }
}
