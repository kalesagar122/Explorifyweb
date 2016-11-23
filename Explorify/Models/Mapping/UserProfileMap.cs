using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.UserProfileId);

            // Properties
            this.Property(t => t.UserId)
                .HasMaxLength(128);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.UserProfileId).HasColumnName("UserProfileId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.YearExperience).HasColumnName("YearExperience");
            this.Property(t => t.MonthExperience).HasColumnName("MonthExperience");

            // Relationships
            //this.HasOptional(t => t.AspNetUser)
            //    .WithMany(t => t.UserProfiles)
            //    .HasForeignKey(d => d.UserId);

        }
    }
}
