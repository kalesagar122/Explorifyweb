using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class UserCategoryMap : EntityTypeConfiguration<UserCategory>
    {
        public UserCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("UserCategory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");

            // Relationships
            //this.HasRequired(t => t.AspNetUser)
            //    .WithMany(t => t.UserCategories)
            //    .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.Category)
                .WithMany(t => t.UserCategories)
                .HasForeignKey(d => d.CategoryId);

        }
    }
}
