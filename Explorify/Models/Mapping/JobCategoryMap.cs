using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class JobCategoryMap : EntityTypeConfiguration<JobCategory>
    {
        public JobCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("JobCategory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.JobCategories)
                .HasForeignKey(d => d.CategoryId);
            this.HasRequired(t => t.Job)
                .WithMany(t => t.JobCategories)
                .HasForeignKey(d => d.JobId);

        }
    }
}
