using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class UserRegIdMap : EntityTypeConfiguration<UserRegId>
    {
        public UserRegIdMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.RegId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UserRegId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RegId).HasColumnName("RegId");

            // Relationships
            //this.HasRequired(t => t.AspNetUser)
            //    .WithMany(t => t.UserRegIds)
            //    .HasForeignKey(d => d.UserId);

        }
    }
}
