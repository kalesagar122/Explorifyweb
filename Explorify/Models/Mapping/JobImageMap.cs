using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class JobImageMap : EntityTypeConfiguration<JobImage>
    {
        public JobImageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ImagePath)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("JobImage");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.ImagePath).HasColumnName("ImagePath");

            // Relationships
            this.HasRequired(t => t.Job)
                .WithMany(t => t.JobImages)
                .HasForeignKey(d => d.JobId);

        }
    }
}
